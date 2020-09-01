using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class ReferenceFinder
{
	ModuleDefinition moduleDefinition;

	public ReferenceFinder(ModuleDefinition moduleDefinition)
	{
		this.moduleDefinition = moduleDefinition;
	}

	public MethodReference GetMethodReference(TypeReference typeReference, Func<MethodDefinition, bool> predicate)
	{
		var typeDefinition = typeReference.Resolve();

		MethodDefinition methodDefinition;
		do
		{
			methodDefinition = typeDefinition.Methods.FirstOrDefault(predicate);
			typeDefinition = typeDefinition.BaseType?.Resolve();
		} while (methodDefinition == null && typeDefinition != null);

		return moduleDefinition.ImportReference(methodDefinition);
	}

	public MethodReference GetOptionalMethodReference(TypeReference typeReference, Func<MethodDefinition, bool> predicate)
	{
		var typeDefinition = typeReference.Resolve();

		MethodDefinition methodDefinition;
		do
		{
			methodDefinition = typeDefinition.Methods.FirstOrDefault(predicate);
			typeDefinition = typeDefinition.BaseType?.Resolve();
		} while (methodDefinition == null && typeDefinition != null);

		return null != methodDefinition ? moduleDefinition.ImportReference(methodDefinition) : null;
	}
}

public class ModuleWeaver : BaseModuleWeaver
{
	public static TypeDefinition AttributeType;
	public TypeDefinition ActivatorTypeRef;
	public TypeDefinition SystemTypeRef;
	public TypeReference MethodBaseTypeRef;
	public TypeReference ExceptionTypeRef;
	public TypeReference OjectTypeRef;

	ReferenceFinder ReferenceFinder;

	public ModuleWeaver()
	{
	}

	public override void Execute()
	{
		ActivatorTypeRef = FindTypeDefinition("System.Activator");
		SystemTypeRef = FindTypeDefinition("System.Type");
		MethodBaseTypeRef = ModuleDefinition.ImportReference(FindTypeDefinition("System.Reflection.MethodBase"));
		ExceptionTypeRef = ModuleDefinition.ImportReference(FindTypeDefinition("System.Exception"));
		OjectTypeRef = ModuleDefinition.ImportReference(TypeSystem.ObjectDefinition);
		ReferenceFinder = new ReferenceFinder(ModuleDefinition);

		var attributes = ModuleDefinition.CustomAttributes.Select(c => c.AttributeType.Resolve());
		AttributeType = attributes.FirstOrDefault();

		ProcessAssembly();
	}

	static IEnumerable<TypeDefinition> GetAllTypes(TypeDefinition type)
	{
		yield return type;

		var allNestedTypes = from t in type.NestedTypes
							 from t2 in GetAllTypes(t)
							 select t2;

		foreach (var t in allNestedTypes)
			yield return t;
	}

	public override IEnumerable<string> GetAssembliesForScanning()
	{
		yield return "netstandard";
		yield return "mscorlib";
		yield return "System";
		yield return "System.Runtime";
		yield return "System.Core";
		yield return "Xamarin.Forms.Core";
	}

	void ReadConfig()
	{
	}

	void ProcessAssembly()
	{
		var allTypes = ModuleDefinition.Types.SelectMany(GetAllTypes)
			.Where(t => t.IsClass && t.IsPublic)
			.Where(t => AttributeType.FullName != t.FullName);

		foreach (var type in allTypes)
		{
			IEnumerable<MethodDefinition> methods = from m in type.Methods
													where m.IsPublic
													select m;
			foreach (var method in methods)
			{
				ProcessMethod(type, method);
			}
		}
	}

	void ProcessMethod(TypeDefinition type, MethodDefinition method)
	{
		if (method.Body == null)
			return;

		method.Body.InitLocals = true;
		var processor = method.Body.GetILProcessor();
		var firstInstruction = method.Body.Instructions.First();

		if (method.IsConstructor)
		{
			var callBase = method.Body.Instructions.FirstOrDefault(
				i => i.OpCode == OpCodes.Call
				&& (i.Operand is MethodReference reference)
				&& reference.Resolve().IsConstructor);

			firstInstruction = callBase?.Next ?? firstInstruction;
		}

		//Init
		var ctorDef = ActivatorTypeRef.Methods.First(m => m.Name == "CreateInstance" && m.Parameters.Count == 1);
		var ctorRef = ModuleDefinition.ImportReference(ctorDef);
		var getTypeDef = SystemTypeRef.Methods.First(m => m.Name == "GetTypeFromHandle" && m.Parameters.Count == 1);
		var getTypeRef = ModuleDefinition.ImportReference(getTypeDef);
		var getMethodDef = ReferenceFinder.GetMethodReference(MethodBaseTypeRef, m => m.Name == "GetMethodFromHandle" && m.Parameters.Count == 2);
		var getMethodRef = ModuleDefinition.ImportReference(getMethodDef);
		var attrRef = ModuleDefinition.ImportReference(AttributeType);

		var variableDef = new VariableDefinition(AttributeType);
		method.Body.Variables.Add(variableDef);

		var methodVariableDef = new VariableDefinition(MethodBaseTypeRef);
		method.Body.Variables.Add(methodVariableDef);

		var instructions = new List<Instruction>();
		instructions.Add(processor.Create(OpCodes.Ldtoken, attrRef));
		instructions.Add(processor.Create(OpCodes.Call, getTypeRef));
		instructions.Add(processor.Create(OpCodes.Call, ctorRef));
		instructions.Add(processor.Create(OpCodes.Castclass, attrRef));
		instructions.Add(processor.Create(OpCodes.Stloc, variableDef));

		processor.InsertBefore(firstInstruction, instructions);
		instructions.Clear();

		var initMethodDef = AttributeType.Methods.Where(m => m.Name.Contains("Init")).FirstOrDefault();
		if (initMethodDef != null)
		{
			var initMethodRef = ModuleDefinition.ImportReference(initMethodDef);

			instructions.Add(processor.Create(OpCodes.Ldloc, variableDef));

			if (initMethodRef.Parameters.Count > 1)
			{
				// then push the instance reference onto the stack
				if (method.IsConstructor || method.IsStatic)
				{
					instructions.Add(processor.Create(OpCodes.Ldnull));
				}
				else
				{
					instructions.Add(processor.Create(OpCodes.Ldarg_0));
					if (type.IsValueType)
					{
						instructions.Add(processor.Create(OpCodes.Box, type));
					}
				}
			}
			instructions.Add(processor.Create(OpCodes.Ldtoken, method));
			instructions.Add(processor.Create(OpCodes.Ldtoken, method.DeclaringType));
			instructions.Add(processor.Create(OpCodes.Call, getMethodDef));
			instructions.Add(processor.Create(OpCodes.Stloc, methodVariableDef));

			instructions.Add(processor.Create(OpCodes.Ldloc, methodVariableDef));
			instructions.Add(processor.Create(OpCodes.Callvirt, initMethodRef));

			processor.InsertBefore(firstInstruction, instructions);
			instructions.Clear();
		}

		//Entry
		var entryMethodDef = AttributeType.Methods.Where(m => m.Name.Contains("OnEntry")).FirstOrDefault();

		if (entryMethodDef != null)
		{
			var entryMethodRef = ModuleDefinition.ImportReference(entryMethodDef);

			instructions.Add(processor.Create(OpCodes.Ldloc, variableDef));
			instructions.Add(processor.Create(OpCodes.Callvirt, entryMethodRef));

			processor.InsertBefore(firstInstruction, instructions);
			instructions.Clear();
		}

		//TODO: OnExit, OnException
	}
}
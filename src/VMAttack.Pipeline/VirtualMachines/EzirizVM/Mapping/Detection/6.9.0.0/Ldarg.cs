﻿using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using VMAttack.Pipeline.VirtualMachines.EzirizVM.Abstraction;
using VMAttack.Pipeline.VirtualMachines.EzirizVM.Architecture;

namespace VMAttack.Pipeline.VirtualMachines.EzirizVM.Mapping.Detection._6._9._0._0;

public static partial class Handler
{
    [DetectV1(CilCode.Ldarg)]
    public static bool Is_LdargPattern(this EzirizOpcode code)
    {
        var ldargPattern = new[]
        {
            CilCode.Ldarg_0, //0
            CilCode.Ldfld, //1
            CilCode.Ldarg_0, //2
            CilCode.Ldfld,  //3
            CilCode.Ldarg_0, //4
            CilCode.Ldfld, //5
            CilCode.Unbox_Any, //6 
            CilCode.Ldelem_Ref,
            CilCode.Callvirt, 
            CilCode.Ret
        };
        
        var handler = code.Handler;
        var instructions = handler.Instructions;

        if (handler.MatchesEntire(ldargPattern))
        {
            if (instructions[6].Operand is not TypeReference type)
                return false;

            var module = type.Module;
            var corLibTypeFactory = module?.CorLibTypeFactory;

            if (type.ToTypeSignature() == corLibTypeFactory?.Int32)
                return true;
        }

        return false;
    }
}
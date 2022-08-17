﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Assertions;

public static class ThreeAddressCodeInterpreter
{
    
    public class LiteralizedThreeAddressCodeException : Exception
    {
        public LiteralizedThreeAddressCodeException(string message, int lineIndex, string line) : base(message)
        {
            Debug.Log($"Found a syntax error on line {lineIndex}: {message}\n{line}\n");
        }
    }
    public static void test(string code, int expected)
    {
        var vm = new EnemyVM();
        foreach (var instruction in interpretCode(code)) vm.appendInstruction(instruction);

        while (!vm.IsExit) vm.run();

        var returnedValue = vm.ReturnValue;
        if (returnedValue != expected)
            throw new AssertionException("Assertion Failed.", $"Expected: {expected} -- Actual: {returnedValue}");
    }
    public static List<EnemyVM.Instruction> interpretCode(string code)
    {
        var instructionSeries = new List<EnemyVM.Instruction>();
        foreach (var (index, line) in Util_Array.Indexed(code.Split("\n")))
        {
            foreach (var statement in line.Split(";"))
            {
                if (IsEmptyLine(statement)) continue;
                var strainedStatement = simplifyCodeLine(statement);
                var instruction = interpretStarlizedLine(strainedStatement, index);
                instructionSeries.Add(instruction);
            }
        }
        return instructionSeries;

    }

    private static bool IsEmptyLine(string line)
    {
        return line.Length == 0 || Regex.IsMatch(line, "^[\\t\\s]*$");
    }

    private static string simplifyCodeLine(string line)
    {
        line = Regex.Replace(line, "^[\\t\\s]+", "");
        line = Regex.Replace(line, "[\\t\\s]+$", "");
        line = Regex.Replace(line, "\\s+", " ");
        return line;
    }

    private static EnemyVM.Instruction interpretStarlizedLine(string line, int lineIndex)
    {
        Regex codeFormat = new Regex("(?<mnemonic>[0-9a-zA-Z]+) (?<argument>[0-9]+)");
        if (!codeFormat.IsMatch(line))
            throw new LiteralizedThreeAddressCodeException("Not correct format.", lineIndex, line);

        Match tokens = codeFormat.Match(line);
        EnemyVM.Mnemonic mnemonic;
        if (!Enum.TryParse(tokens.Groups["mnemonic"].Value, out mnemonic))
            throw new LiteralizedThreeAddressCodeException($"Mnemonic {tokens.Groups["mnemonic"].Value} is not defined.", lineIndex, line);

        return new EnemyVM.Instruction(mnemonic, int.Parse(tokens.Groups["argument"].Value));
    }
}
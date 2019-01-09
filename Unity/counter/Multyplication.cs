using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multyplication : Operation {

    protected override void AwakeChild() {
        getStringSolution = () => {
            return num1 + " * " + num2 + " = ";
        };

        getAnswer = () => {
            return num1 * num2;
        };
    }

    //public override string GetSolution(out int answer) {
    //    GetRandomNumbers();
    //    answer = num1 * num2;
    //    return num1 + " * " + num2 + " = ";
    //}
}

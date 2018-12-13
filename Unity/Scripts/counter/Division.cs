using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Division : Operation {

    protected override void AwakeChild() {
        getStringSolution = () => {
            return num1 + " ÷ " + num2 + " = ";
        };

        getAnswer = () => {
            int x;
            x = num1 * num2;
            return x / num2;
        };
    }


    //public override string GetSolution(out int answer) {
    //    int x;
    //    GetRandomNumbers();

    //    x = num1 * num2;
    //    answer = x / num2;

    //    return x + " ÷ " + num2 + " = ";
    //}
}

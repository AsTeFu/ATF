using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plus : Operation {

    protected override void AwakeChild() {
        
        getStringSolution = () => {
            return num1 + " + " + num2 + " = ";
        };

        getAnswer = () => {
            return num1 + num2;
        };
    }

    //answer = num1 + num2;
}

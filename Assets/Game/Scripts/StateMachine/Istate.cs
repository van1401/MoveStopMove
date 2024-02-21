using System.Collections;
using System.Collections.Generic;
using UniRx.Examples;
using UnityEngine;

public interface IState
{
    void OnEnter(Enemy enemy);



    void OnExecute(Enemy enemy);



    void OnExit(Enemy enemy);


}

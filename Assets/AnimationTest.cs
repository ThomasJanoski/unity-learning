using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    IEnumerator coroutine;
    Animation Anim;
    string State = "Crate_Open";
    bool CanOpen = true;
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    float GetAnimationLength(string StateName, Animation Anim) {
         float length = 0;
        foreach(AnimationState state in Anim) {
            if (state.name.Equals(State)) {length = state.length;}
        }
        return length;
    }
    private IEnumerator ChangeOpenBool(float Length) {
        yield return new WaitForSecondsRealtime(Length);
        CanOpen = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) & CanOpen) {
            CanOpen = false;
            Anim.Play(State);
            float length = GetAnimationLength(State, Anim);
            
            State = State.Equals("Crate_Open") ? "Crate_Close" : "Crate_Open";

            coroutine = ChangeOpenBool(length);
            StartCoroutine(coroutine);
        }
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class NonGod_Card
//{
//    public string correspondingGod;
//    public short baseStrength;

//    public CardType cardType;

//    public override Card_Behaviour Init(GameObject obj)
//    {
//        Card_Behaviour behaviour = null; 
//        switch(cardType)
//        {
//            case CardType.Attack:
//                behaviour = setBehaviour<Attack_Behaviour>(obj);
//                break;

//            case CardType.Defence:
//                behaviour = setBehaviour<Defense_Behaviour>(obj);
//                break;

//            case CardType.Buff:
//                behaviour = setBehaviour<Buff_Behaviour>(obj);
//                break;
//        }
//        return behaviour;
//    }

//    T setBehaviour<T>(GameObject obj) where T : NonGod_Behaviour
//    {
//        T behaviour = obj.AddComponent<T>();
//        behaviour.Initialize(this);
//        return behaviour;
//    }
//}

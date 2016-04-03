﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace eeGames.Actor
{
    public enum LoopType
    {
        PingPong,
        StartOver
    }
    public enum ActingType
    {
        Scale,
        Rotation,
        Position,
        Color
    }
    // 
    [System.Serializable]
    public class Actor : MonoBehaviour 
    {
        public ActingType Type;
        public ActorData ActorData = new ActorData();
        public ActorEvent OnStart;
        public ActorEvent OnStop;

     
        void Start() 
        {
            // here add auto play functionality
            if (ActorData.IsAutoPlay) PerformActing();
           
        }

    
       
         [ContextMenu("Perform Acting")]
        public void PerformActing()
        {
            if (!ActorData.IsActive) return;
            switch (Type)
            {
            
                case ActingType.Scale:
                    DoScaleActing();
                    break;
                case ActingType.Rotation:
                    DoRotationActing();
                    break;
                case ActingType.Position:
                    DoPositionActing();
                    break;
                case ActingType.Color:
                    DoColorActing();
                    break;
            }
        }


        #region Helper Acting Methods
       
        private void DoScaleActing() 
        {
            if (OnStart != null) OnStart.Invoke();
            var mainWindow = GetComponent<RectTransform>();
            mainWindow.transform.localScale = ActorData.From;
            LTDescr id = LeanTween.scale(mainWindow, ActorData.To, ActorData.Time).setDelay(ActorData.DelayTime).setLoopPingPong(ActorData.IsLoop ? -1 : ActorData.TweenCount).setEase(ActorData.TweenType).setOnComplete(() => { if (OnStop != null) OnStop.Invoke(); });
        }


        private void DoPositionActing()
        {

            if (OnStart != null) OnStart.Invoke();
            var mainWindow = GetComponent<RectTransform>();
            mainWindow.transform.position = ActorData.From;

#if UNITY_EDITOR
            string[] dimension = UnityEditor.UnityStats.screenRes.Split('x');
            int _width = System.Int32.Parse(dimension[0]);
            int _height = System.Int32.Parse(dimension[1]);
#endif

#if !UNITY_EDITOR
        int _width = Screen.width;
        int _height = Screen.height;
#endif

            var newPos = ActorData.From;
            newPos.x *= _width;
            newPos.y *= _height;
            mainWindow.transform.position = newPos;

            var pos = ActorData.To;
            pos.x *= _width;
            pos.y *= _height;


            if (ActorData.IsLoop)
            {
                LTDescr id = LeanTween.move(mainWindow.gameObject, (Vector3)pos, ActorData.Time).setDelay(ActorData.DelayTime).setLoopPingPong(ActorData.IsLoop ? -1 : ActorData.TweenCount).setEase(ActorData.TweenType).setOnComplete(() => { if (OnStop != null) OnStop.Invoke(); });
            }
            else
            {
                LTDescr id = LeanTween.move(mainWindow.gameObject, (Vector3)pos, ActorData.Time).setDelay(ActorData.DelayTime).setLoopOnce().setEase(ActorData.TweenType).setOnComplete(() => { if (OnStop != null) OnStop.Invoke(); });
            }


           
        }


        private void DoRotationActing()
        {
            
            if (OnStart != null) OnStart.Invoke();
            var mainWindow = GetComponent<RectTransform>();
            mainWindow.transform.rotation = Quaternion.Euler(ActorData.From);
            LTDescr id = LeanTween.rotate(mainWindow.gameObject, ActorData.To, ActorData.Time).setDelay(ActorData.DelayTime).setLoopPingPong(ActorData.IsLoop ? -1 : ActorData.TweenCount).setEase(ActorData.TweenType).setOnComplete(() => { if (OnStop != null) OnStop.Invoke(); });
        }




        private void DoColorActing()
        {

            if (OnStart != null) OnStart.Invoke();
            var mainWindow = GetComponent<RectTransform>();
            
            
            var uiImg = GetComponent<Image>();
            if (uiImg != null)
            {
                LTDescr id = LeanTween.color(mainWindow, ActorData.To, ActorData.Time).setDelay(ActorData.DelayTime).setLoopPingPong(ActorData.IsLoop ? -1 : ActorData.TweenCount).setEase(ActorData.TweenType).setOnComplete(() => { if (OnStop != null) OnStop.Invoke(); });
            }

            var canvasGroup = GetComponent<CanvasGroup>();
         //   if (ActorData.To.z != ActorData.From.z)
            {
                // lets do the alpha business
                LeanTween.value(mainWindow.gameObject, (alpha) => { canvasGroup.alpha = alpha; Debug.Log("here is alpha " + alpha); }, ActorData.From.z, ActorData.To.z, ActorData.Time).setDelay(ActorData.DelayTime).setLoopPingPong(ActorData.IsLoop ? -1 : ActorData.TweenCount).setEase(ActorData.TweenType).setOnComplete(() => { if (OnStop != null) OnStop.Invoke(); });
            }
        }
        #endregion
    }

    /// <summary>
    /// data structure used to store tween data
    /// </summary>
    [System.Serializable]
    public class ActorData
    {
        public bool IsActive;
        public float Time;
        public float DelayTime;
        public int TweenCount;
        public bool IsAutoPlay;
        public bool IsLoop;
        public LeanTweenType TweenType;
        public LoopType LoopType;
        public Vector4 From;
        public Vector4 To;
//        public Vector4 Hide;
    }

    [System.Serializable]
    public class ActorEvent : UnityEngine.Events.UnityEvent
    { }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(AutomatedPlatform))]
public class AutomatedPlatformEditor : Editor
{
    AutomatedPlatform automatedPlatform;
    bool needsRepaint;
    bool snapOn;
    SelectionInfo selectionInfo;

    private void OnEnable()
    {
        automatedPlatform = (AutomatedPlatform)target;
        selectionInfo = new SelectionInfo();
        snapOn = false;
    }

    public override void OnInspectorGUI()
    {
        
        if (automatedPlatform.pathPoints.Count < 2)
        {
            automatedPlatform.CreateDefaultPath();
        }
        if (automatedPlatform.pathPoints.Count < 3)
        {
            automatedPlatform.mode = PLATFORM_MODE.YOYO;
        }
        if (DrawDefaultInspector())
        {
            switch (automatedPlatform.mode)
            {
                case PLATFORM_MODE.YOYO:
                    automatedPlatform.pathPoints[automatedPlatform.pathPoints.Count - 1].nextPoint = Vector2.negativeInfinity;
                    break;
                case PLATFORM_MODE.CYCLIC:
                    automatedPlatform.pathPoints[automatedPlatform.pathPoints.Count - 1].nextPoint = automatedPlatform.pathPoints[0].startPoint;
                    break;
                default:
                    break;
            }
            

            for(int i=0;i<automatedPlatform.pathPoints.Count;i++)
            {
                automatedPlatform.pathPoints[i].chainSeparation = automatedPlatform.chainDistance>0? automatedPlatform.chainDistance:0.5f;
                automatedPlatform.pathPoints[i].ReconfigureChains();
            }
            
            if (automatedPlatform.switchOperated)
            {
                if (automatedPlatform.operatorSwitchInstance == null)
                {                    
                    automatedPlatform.operatorSwitchInstance = Instantiate(automatedPlatform.operatorSwitchPrefab, automatedPlatform.transform.position, Quaternion.identity, automatedPlatform.transform);
                }
            }
            else
            {
                if (automatedPlatform.operatorSwitchInstance != null)
                {
                    DestroyImmediate(automatedPlatform.operatorSwitchInstance);
                }
            }
            
            HandleUtility.Repaint();
        }
    }

    private void OnSceneGUI()
    {
        Event guiEvent = Event.current;
        if(guiEvent.type == EventType.Repaint)
        {
            Draw();
        }
        else if (guiEvent.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
        else
        {
            HandleInput(guiEvent);
            if (needsRepaint)
            {
                HandleUtility.Repaint();
                needsRepaint = false;
            }
        }
    }

    void HandleInput(Event guiEvent)
    {
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        float drawPlaneHeight = 0;
        float dstToDrawPlane = (drawPlaneHeight - mouseRay.origin.z) / mouseRay.direction.z;
        Vector3 mousePosition = mouseRay.GetPoint(dstToDrawPlane);

        if (guiEvent.modifiers == EventModifiers.Shift)
        {
            snapOn = true;
        }
        else
        {
            snapOn = false;
        }

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
        {
            HandleLeftMouseDown(mousePosition);
        }

        if (guiEvent.type == EventType.MouseUp && guiEvent.button == 0)
        {
            
            HandleLeftMouseUp(mousePosition);
        }

        if (guiEvent.type == EventType.MouseDrag && guiEvent.button == 0)
        {
            HandleLeftMouseDrag(mousePosition);
        }

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1 && guiEvent.modifiers == EventModifiers.None)
        {
            HandleRightMouseDown();
        }

        if (!selectionInfo.pointIsSelected)
        {
            UpdateMouseOverInfo(mousePosition);
        }
    }

    void HandleLeftMouseDown(Vector3 mousePosition)
    {
        Vector2 position = SnapCorrection(mousePosition, automatedPlatform.pathPoints[automatedPlatform.pathPoints.Count - 1].startPoint);
        if (!selectionInfo.mouseIsOverPoint)
        {
            Undo.RecordObject(automatedPlatform, "Add point");
            automatedPlatform.AddPoint(position);
            selectionInfo.pointIndex = automatedPlatform.pathPoints.Count - 1;
        }

        selectionInfo.pointIsSelected = true;
        selectionInfo.positionAtStartOfDrag = position;
        needsRepaint = true;
    }
    void HandleLeftMouseUp(Vector3 mousePosition)
    {
        if(selectionInfo.pointIsSelected)
        {
            automatedPlatform.pathPoints[selectionInfo.pointIndex].startPoint = selectionInfo.positionAtStartOfDrag;
            Undo.RecordObject(automatedPlatform,"Move point");
            Vector2 position = mousePosition;
            if (selectionInfo.pointIndex> 0)
            {
                position = SnapCorrection(mousePosition, automatedPlatform.pathPoints[selectionInfo.pointIndex - 1].startPoint);   
            }
            automatedPlatform.pathPoints[selectionInfo.pointIndex].startPoint = position;
            automatedPlatform.pathPoints[selectionInfo.pointIndex].ReconfigureChains();
            if (selectionInfo.pointIndex - 1 >= 0)
            {
                automatedPlatform.pathPoints[selectionInfo.pointIndex - 1].nextPoint = position;
                automatedPlatform.pathPoints[selectionInfo.pointIndex - 1].ReconfigureChains();
            }
            else if(selectionInfo.pointIndex==0)
            {
                automatedPlatform.transform.position = automatedPlatform.pathPoints[0].startPoint;
                if(automatedPlatform.mode == PLATFORM_MODE.CYCLIC)
                {
                    automatedPlatform.pathPoints[automatedPlatform.pathPoints.Count - 1].nextPoint = automatedPlatform.pathPoints[0].startPoint;
                    automatedPlatform.pathPoints[automatedPlatform.pathPoints.Count - 1].ReconfigureChains();
                }
                
            }
            selectionInfo.pointIsSelected = false;
            selectionInfo.pointIndex = -1;
            needsRepaint = true;
        }
    }
    void HandleLeftMouseDrag(Vector3 mousePosition)
    {
        if(selectionInfo.pointIsSelected)
        {
            Vector2 position = mousePosition;
            if (selectionInfo.pointIndex > 0)
            {
                position = SnapCorrection(mousePosition, automatedPlatform.pathPoints[selectionInfo.pointIndex - 1].startPoint);
            }
            automatedPlatform.pathPoints[selectionInfo.pointIndex].startPoint = position;
            
            needsRepaint = true;
        }
    }

    void HandleRightMouseDown()
    {
        if (selectionInfo.mouseIsOverPoint)
        {
            Undo.RecordObject(automatedPlatform, "Remove point");
            automatedPlatform.RemovePointAt(selectionInfo.pointIndex);
            needsRepaint = true;
        }
    }

    void UpdateMouseOverInfo(Vector3 mousePosition)
    {
        int mouseOverPointIndex = -1;
        for(int i=0;i<automatedPlatform.pathPoints.Count;i++)
        {
            if(Vector3.Distance(mousePosition, automatedPlatform.pathPoints[i].startPoint)< automatedPlatform.discSize)
            {
                mouseOverPointIndex = i;
                break;
            }
        }
        if(mouseOverPointIndex != selectionInfo.pointIndex)
        {
            selectionInfo.pointIndex = mouseOverPointIndex;
            selectionInfo.mouseIsOverPoint = mouseOverPointIndex != -1;

            needsRepaint = true;
        }
    }

    void Draw()
    {
        for (var i = 0; i < automatedPlatform.pathPoints.Count; i++)
        {
            Vector2 nextPoint = Vector2.zero;
            if (automatedPlatform.mode == PLATFORM_MODE.YOYO)
            {
                if (i < automatedPlatform.pathPoints.Count - 1)
                {
                    nextPoint = automatedPlatform.pathPoints[i + 1].startPoint;
                    Handles.color = Color.yellow;
                    Handles.DrawDottedLine(automatedPlatform.pathPoints[i].startPoint, nextPoint, 4);
                }
            }
            else
            {
                nextPoint = automatedPlatform.pathPoints[(i + 1) % automatedPlatform.pathPoints.Count].startPoint;
                Handles.color = Color.yellow;
                Handles.DrawDottedLine(automatedPlatform.pathPoints[i].startPoint, nextPoint, 4);
            }
            //Draw Chains
            if (!selectionInfo.pointIsSelected)
            {
                if (automatedPlatform.pathPoints[i].chainPoints.Count != 0)
                {
                    Handles.color = Color.white;
                    for (int j = 0; j < automatedPlatform.pathPoints[i].chainPoints.Count; j++)
                    {
                        Handles.DrawSolidDisc(automatedPlatform.pathPoints[i].chainPoints[j], Vector3.forward, automatedPlatform.chainSize);
                    }
                }
            }
            else
            {
                if (automatedPlatform.mode == PLATFORM_MODE.YOYO)
                {
                    if (selectionInfo.pointIsSelected && i != selectionInfo.pointIndex && i != Mathf.Max(selectionInfo.pointIndex - 1, 0))
                    {
                        if (automatedPlatform.pathPoints[i].chainPoints.Count != 0)
                        {
                            Handles.color = Color.white;
                            for (int j = 0; j < automatedPlatform.pathPoints[i].chainPoints.Count; j++)
                            {
                                Handles.DrawSolidDisc(automatedPlatform.pathPoints[i].chainPoints[j], Vector3.forward, automatedPlatform.chainSize);
                            }
                        }
                    }
                }
                else if(automatedPlatform.mode == PLATFORM_MODE.CYCLIC)
                {
                    int previous = (selectionInfo.pointIndex - 1)%automatedPlatform.pathPoints.Count;
                    if(previous<0)
                    {
                        previous += automatedPlatform.pathPoints.Count;
                    }

                    if (selectionInfo.pointIsSelected && i != selectionInfo.pointIndex && i != previous)
                    {
                        if (automatedPlatform.pathPoints[i].chainPoints.Count != 0)
                        {
                            Handles.color = Color.white;
                            for (int j = 0; j < automatedPlatform.pathPoints[i].chainPoints.Count; j++)
                            {
                                Handles.DrawSolidDisc(automatedPlatform.pathPoints[i].chainPoints[j], Vector3.forward, automatedPlatform.chainSize);
                            }
                        }
                    }
                    
                }
            }


            if (i == selectionInfo.pointIndex)
            {
                Handles.color = (selectionInfo.pointIsSelected) ? Color.green:Color.yellow;
            }
            else
            {
                Handles.color = Color.red;
            }
            Handles.DrawSolidDisc(automatedPlatform.pathPoints[i].startPoint, Vector3.forward, automatedPlatform.discSize);
        }
    }

    Vector2 SnapCorrection(Vector2 position,Vector2 aboutPoint)
    {
        if (snapOn)
        {
            Vector2 dir = position - aboutPoint;
            float distance = Vector2.Distance(position, aboutPoint);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float correctedAngle = Mathf.RoundToInt(angle / automatedPlatform.snapAngle) * automatedPlatform.snapAngle;
            return aboutPoint + new Vector2(distance * Mathf.Cos(correctedAngle * Mathf.Deg2Rad), distance * Mathf.Sin(correctedAngle * Mathf.Deg2Rad));
        }
        return position;
    }


    public class SelectionInfo
    {
        public Vector3 positionAtStartOfDrag;
        public int pointIndex = -1;
        public bool mouseIsOverPoint;
        public bool pointIsSelected;
    }
}

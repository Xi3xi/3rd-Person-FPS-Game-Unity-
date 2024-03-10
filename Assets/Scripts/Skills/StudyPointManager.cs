using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudyPointManager : MonoBehaviour
{
    public static StudyPointManager Instance;
    
    public delegate void StudyPointChange(int studyPoint);
    public event StudyPointChange OnStudyPointChange;

    private int currentStudyPoints = 0;

    // Singleton
    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;
        }
        OnStudyPointChange?.Invoke(currentStudyPoints);
    }

    public void AddStudyPoint(int studyPoint)
    {
        currentStudyPoints += studyPoint;
        OnStudyPointChange?.Invoke(studyPoint);
    }

    public void RemoveStudyPoint(int studyPoint)
    {
        int temp = currentStudyPoints;
        temp -= studyPoint;

        if (temp < 0)
        {
            studyPoint = currentStudyPoints;
            currentStudyPoints = 0;
        }
        else
        {
            currentStudyPoints = temp;
        }
        OnStudyPointChange?.Invoke(-studyPoint);
    }

    public int GetCurrentStudyPoints()
    {
        return currentStudyPoints;
    }
}

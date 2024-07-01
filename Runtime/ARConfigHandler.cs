using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SubsystemsImplementation;

public class ARConfigHandler
{

    public static ARConfigHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (syncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new ARConfigHandler();
                    }
                }

            }
            return _instance;

        }
    }
    private static ARConfigHandler _instance;
    private static readonly object syncRoot = new object();
    private Dictionary<SubsystemProvider, Feature> m_features;
    public Feature allFeatures
    {
        private set;
        get;
    }

    private ARConfigHandler()
    {
        m_features = new Dictionary<SubsystemProvider, Feature>();
    }

    public void AddFeature(SubsystemProvider subsystemProvider, Feature feature)
    {
        Debug.Log("add Feature:"+subsystemProvider+" "+feature);
        if (!m_features.ContainsKey(subsystemProvider))
        {
            m_features.Add(subsystemProvider, feature);
        }
        else
        {
            m_features[subsystemProvider] = feature;
        }
        allFeatures = Feature.None;
        foreach (var item in m_features)
        {
            allFeatures |= item.Value;
        }


    }
    public void RemoveFeature(SubsystemProvider subsystemProvider)
    {
        if (m_features.ContainsKey(subsystemProvider))
        {
            m_features.Remove(subsystemProvider);
            allFeatures = Feature.None;
            foreach (var item in m_features)
            {
                allFeatures |= item.Value;
            }
        }

    }
}

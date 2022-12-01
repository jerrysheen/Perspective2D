using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
        public static ItemManager Instance { get; private set; }
        private Dictionary<ItemBase.ItemType, float> m_ItemDic;       
        private void Awake() 
        { 
            // If there is an instance, and it's not me, delete myself.
    
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else
            {
                OnCreateSystem();
            } 
            
        }

        public void OnCreateSystem()
        {
            Instance = this;
            m_ItemDic = new Dictionary<ItemBase.ItemType, float>();
        }

        public void OnEnableSystem()
        {
           
        }

        public void OnCloseSystem()
        {
        }

        public void OnDestroySystem()
        {
            Destroy(this);
        }

        /// <summary>
        ///  use value negative/positive to control add or sub.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public void ChangeItemValue(ItemBase.ItemType type, float value)
        {
            if (m_ItemDic.ContainsKey(type))
            {
                m_ItemDic[type] += value;
            }
            else
            {
                m_ItemDic[type] = value;
            }
        }

        private void Update()
        {
            if (Instance != null && Instance.m_ItemDic != null)
            {
                if (m_ItemDic.ContainsKey(ItemBase.ItemType.COIN))
                {
                    Debug.Log(m_ItemDic[ItemBase.ItemType.COIN]);
                }
            }
        }
}
    


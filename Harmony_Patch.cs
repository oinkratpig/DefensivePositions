using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DefensivePositions
{
    public class Harmony_Patch
    {
        private static Dictionary<AgentModel, PassageObjectModel> _returnPositions;
        private static Dictionary<AgentModel, PassageObjectModel> _defensePositions;

        // Constructor
        public Harmony_Patch()
        {
            Oink.Log("Loading...");

            HarmonyInstance harmony = HarmonyInstance.Create("oinkratpig.LobotomyCorp.DefensivePositions");
            try
            {
                MethodInfo method = typeof(Harmony_Patch).GetMethod("UnitMouseEventManagerPatch");
                harmony.Patch(typeof(UnitMouseEventManager).GetMethod("Update", AccessTools.all), null, new HarmonyMethod(method), null);
            }
            catch(Exception e)
            {
                Oink.Log(e.Message);
            }

            Oink.Log("Finished patches.");

        } // end constructor

        // Input
        public static void UnitMouseEventManagerPatch(UnitMouseEventManager __instance)
        {
            // Return
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                    _returnPositions = GetAllAgentPositions();
                else
                    SetAllAgentPositions(_returnPositions);
            }
            // Defense
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                    _defensePositions = GetAllAgentPositions();
                else
                    SetAllAgentPositions(_defensePositions);
            }

        } // end UnitMouseEventManagerPatch

        /// <summary>
        /// Get the positions of every agent.
        /// </summary>
        private static Dictionary<AgentModel, PassageObjectModel> GetAllAgentPositions()
        {
            Dictionary<AgentModel, PassageObjectModel> dic = new Dictionary<AgentModel, PassageObjectModel>();
            foreach(AgentUnit agent in AgentLayer.currentLayer.GetAgentList())
            {
                if (agent == null) continue;

                AgentModel model = agent.model;
                if (model == null) continue;
                
                dic.Add(model, Traverse.Create(model).Field("_currentWaitingPassage").GetValue<PassageObjectModel>());
            }
            return dic;

        } // end GetAllAgentPositions

        /// <summary>
        /// Sets the positions of all agents.
        /// </summary>
        private static void SetAllAgentPositions(Dictionary<AgentModel, PassageObjectModel> positions)
        {
            foreach (AgentModel agent in positions.Keys)
                if (agent != null)
                {
                    agent.SetWaitingPassage(positions[agent]);
                    PassageObject passageObject = SefiraMapLayer.instance.GetPassageObject(positions[agent]);
                    if (passageObject != null)
                    {
                        passageObject.OnPointEnter();
                        passageObject.OnPointerClick();
                    }
                }
            
        } // end SetAllAgentPositions

    } // end class

} // end namespace
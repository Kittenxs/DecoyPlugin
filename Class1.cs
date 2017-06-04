using PluginsCommon;
using Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueCarry_
{
    public class Core : IPlugin
    {
        public string Name => "The Decoy Plugin";

        public string Author => "Kritters";

        public Version Version => new Version(1,0,0);

        public string Description => "...";

        public void OnButtonClick()
        {
        }

        public void OnStart()
        {
        }

        public void OnStop(bool off)
        {
        }

        public void Pulse()
        {
            Skandia.Update();

            //Checking GameState
            if(Skandia.Me == null || !Skandia.IsInGame)
            {
                return;
            }

            //Check the Player MainClass
            if(Skandia.Me.Info.MainClass != PlayerClass.Gunslinger)
            {
                Skandia.MessageLog("You have to be a MainClass Gunslinger...");
                return;
            }

            //Check if Skill Exists
            var givenSkill = Skandia.Me.Skills.FirstOrDefault(x => x.Value.Id == 51906u);
            if(givenSkill.Value == null)
            {
                Skandia.MessageLog("You don't have this skill... so you can't use it ");
                return;
            }

            //Logic there
            var xTarget = ObjectManager.ObjectList.FirstOrDefault(x => x.IsEnemy && x.Template != null && (x.Template.TargetType == TargetType.Elite || x.Template.TargetType == TargetType.Monster) && Skandia.Me.Location3D.Distance(x.Location3D) <= 20);
            if(xTarget != null)
            {
                int entitiesCount = ObjectManager.ObjectList.Count((x => x.IsEnemy && x.Template != null && 
                                                                        (x.Template.TargetType == TargetType.Monster || x.Template.TargetType == TargetType.Elite) 
                                                                         &&  (Skandia.Me.Location3D.Distance(x.Location3D) <= 20) || x.Location3D.Distance(xTarget.Location3D) <= 20));
                bool bossCheck = ObjectManager.ObjectList.Exists(x => x.IsEnemy && x.Template != null && (x.Template.TargetType == TargetType.Boss && x.Location3D.Distance(xTarget.Location3D) <= 17));
                if (bossCheck || entitiesCount >= 3)
                {
                    Skandia.Me.SetTarget(xTarget.Guid);
                    if (Skandia.Me.CanSendSkill(51906))
                    {
                        Skandia.Me.SendSkill(51906, false);
                        return;
                    }
                }
            }
           



        }
    }
}

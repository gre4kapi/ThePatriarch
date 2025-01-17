using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ThePatriarch;
partial class Hooks
{
    public static void ApplyMoon()
    {
        On.SLOracleBehaviorHasMark.MoonConversation.AddEvents += MoonOverride;
    }
    public static void MoonOverride(On.SLOracleBehaviorHasMark.MoonConversation.orig_AddEvents orig, SLOracleBehaviorHasMark.MoonConversation self)
    {
        orig(self);
        if (CustomConversations.TryGet(self.myBehavior.oracle.room.game, out bool value) && value)
        {
            if (self.id == Conversation.ID.MoonFirstPostMarkConversation)
            {
                self.events = new List<Conversation.DialogueEvent>();
                switch (Mathf.Clamp(self.State.neuronsLeft, 0, 5)) //this gets the number of neurons left for moon.
                {
                    /*case 0:
                        break;
                    case 1:
                        self.events.Add(new Conversation.TextEvent(self, 0, "...", 0));
                        return;
                    case 2:
                        self.events.Add(new Conversation.TextEvent(self, 0, "... 2 NEURONS LEFT!", 0));
                        return;
                    case 3:
                        self.events.Add(new Conversation.TextEvent(self, 0, "3 NEURON CONVO!", 0));
                        return;
                    case 4:
                        self.events.Add(new Conversation.TextEvent(self, 0, "4 NEURON CONVO!!!", 0));
                        return;*/
                    case 5:
                        self.events = new List<Conversation.DialogueEvent>() {
                        new Conversation.TextEvent(self, 0, Translate("Hello little creature."), 0),
                        new Conversation.TextEvent(self, 0, Translate("Your shape is familiar to me. I feel as if I... might have met your kind before."), 0),
                        new Conversation.TextEvent(self, 0, Translate("But my memory is so unreliable now."), 0),
                        new Conversation.TextEvent(self, 0, Translate("I see that someone has given you the gift of communication. Must have been Five Pebbles, as you don't look like you can travel very far..."), 0),
                        new Conversation.TextEvent(self, 0, Translate("He's sick, you know. Being corrupted from the inside by his own experiments.<LINE> Maybe they all are by now, who knows. We weren't designed to transcend and it drives us mad."), 0),
                        new Conversation.TextEvent(self, 0, Translate("Hm..."), 0),
                        new Conversation.TextEvent(self, 0, Translate("Such a familiar feeling."), 0)
                        };
                        return;

                }
            }
        }
    }
}



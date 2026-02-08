=== npc ===
{npc > 1: -> casual}
It's dangerous outside. #speaker:NPC #sprite:agnes
Take this with you. #speaker:NPC
-> choices

== choices

* [Take]
    You got an item.
* [Leave]
    You declined the NPC.
- -> END

== casual
The NPC has nothing to say.
-> END
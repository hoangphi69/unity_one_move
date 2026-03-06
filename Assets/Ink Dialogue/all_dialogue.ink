=== npc ===
{npc > 1: -> casual}
It's dangerous outside. #speaker:NPC #sprite:agnes
Take this with you. #speaker:Phạm Nhật Vượng #sprite:agnes
-> choices

== choices

* [Lấy luôn em ơi]
    You got an item.
* [Thôi em ơi]
    You declined the NPC.
- -> END

== casual
The NPC has nothing to say.
-> END
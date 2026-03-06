// =============================================
// CHAPTER 1: KHI CÒN CHƯA BIẾT ĐI VỀ ĐÂU
// =============================================
VAR interact_phone = false
VAR accept_invitation = false
VAR talked_discuss = false
VAR talked_hallway2 = false
VAR talked_hallway3 = false
VAR talked_libarian = false
VAR pick_any_book = false
VAR chose_book1 = false
VAR chose_book2 = false
VAR chose_book3 = false

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

=== ch1_Cutscene1 ===
Màn hình máy tính sáng lên trong căn phòng trọ bao bọc trong tĩnh lặng và bóng tối. 
Chỉ có tiếng nhạc game đơn điệu phát ra từ máy tính cùng âm thanh gõ bàn phím lạch cạch của Nam. #bg:dom

Thực tập xong rồi. Môn cũng hết rồi. #speaker:Nam #sprite:nam_thinking
Mọi thứ đáng lẽ phải nhẹ nhõm hơn chứ? #speaker:Nam #sprite:nam_thinking
Vậy mà sao mình lại thấy trống rỗng thế này... #speaker:Nam #sprite:nam_bored

* [Tiếp tục chơi game]
    Cứ chơi thêm một ván nữa. Rồi lại một ván nữa. #bg:black
    Cái cảm giác vô định này... #speaker:Nam #sprite:nam_bored
    Mình không thích nó!! #speaker:Nam #sprite:nam_angry
    Haizzz... Chán thật. #speaker:Nam #sprite:nam_bored #bg:dom
    -> touchgrass

* [Nhìn ra ngoài cửa sổ]
    Sau khi kéo rèm cửa, ánh sáng chiếu rọi khắp phòng, trải lên khuôn mặt Nam một màu vàng nhạt. #bg:dom_pull_curtain
    Mình đang làm gì với đời vậy nhờ?? #speaker:Nam #sprite:nam_thinking
    -> touchgrass

= touchgrass
Không thể cứ lầm lì mãi một chỗ được. #speaker:Nam #sprite:nam_thinking
Ra ngoài đường thư giãn đầu óc thôi. #speaker:Nam #sprite:nam_thinking
#bg:nam_touchgrass
-> DONE

=== ch1_Lobby1 ===
{ interact_phone == false:
    ~ interact_phone = true

    _"Ting!!!!!"_
    "Bạn có một thông báo mới từ Discord." #bg:black_with_phone

    "Êy cu, làm đồ án tốt nghiệp với tao ko?" #speaker:Phong 
    "Đằng nào tao với mày cũng xong mấy môn sớm," #speaker:Phong 
    "Thì tại sao hai tụi mình ko làm đồ án sớm luôn chứ nhỉ??" #speaker:Phong

    Uầyyy!!! Nó rủ mình làm đồ án chung này. #speaker:Nam #sprite:nam_surprise
    Cơ mà mình lỡ thất bại thì sao? #speaker:Nam #sprite:nam_thinking
    Nếu mình ko đủ giỏi thì sao? #speaker:Nam #sprite:nam_thinking
    Lỡ mà kéo nó xuống chung với mình thì sao? #speaker:Nam #sprite:nam_thinking

    "Quen mày lâu rồi, tao hiểu tính mày nên tao mới rủ đấy" #speaker:Phong
    "Chứ mấy đứa khác tao không yên tâm. Làm đồ án chung mà không hợp cạ mệt lắm" #speaker:Phong
    "Thế chú có tính làm ko?" #speaker:Phong 

    + [Đồng ý]
        -> decision

    + [Lưỡng lự]
        ... #speaker:Nam #sprite:nam_confused
        Đằng nào cũng đang rảnh chán. #speaker:Nam #sprite:nam_talk
        -> decision

- else:
    Hiện tại bạn có 0 thông báo.
    -> DONE
}

= decision
"Kay" #speaker:Nam #sprite:nam_smile
"Công việc thế nào hả, cộng sự" #speaker:Nam #sprite:nam_smile
~ accept_invitation = true
-> DONE


// ----------- Cutscene sau lobby1 ----------
=== ch1_OneWeekLater ===
#bg:one_week_later
-> ch1_Cutscene2

=== ch1_Cutscene2 ===
Đồ án làm thử AI không? #speaker:Phong #sprite:phong_talk 
Hả? Làm thử? Mày tính làm thử AI á? #speaker:Nam #sprite:nam_talk
Mày biết đồ án AI nặng cỡ nào không?  #speaker:Nam #sprite:nam_talk
Từ lọc cả ngàn ảnh, đánh dấu vật thể, gắn tag cho từng ảnh thôi là cực hình rồi. #speaker:Nam #sprite:nam_talk
Chưa tính đến code hay ý tưởng đề tài phải thật sáng tạo thì mày nhắm 2 đứa làm nổi không? #speaker:Nam #sprite:nam_talk
... #speaker:Phong #sprite:phong_thinking
...Mày nói đúng, còn ý tưởng nào khác không? #speaker:Phong #sprite:phong_talk

Thế sao hai đứa không thử làm Web? #speaker:Nam #sprite:nam_thinking
Ờ, rồi làm giống mấy chục nhóm khác. #speaker:Phong #sprite:phong_talk
Xong mang lên hội đồng so xem ai đẹp hơn à? #speaker:Phong #sprite:phong_talk
Chán lắm, đổi món đi!! #speaker:Phong #sprite:phong_bored

Thế Blockchain thì sao? #speaker:Phong #sprite:phong_talk
Lỡ mà thành công thì hai thằng tung luôn coin ra thị trường luôn. #speaker:Phong #sprite:phong_smile
Vừa có đồ án tốt nghiệp vừa có dự án khởi nghiệp. #speaker:Phong #sprite:phong_smile
Hehe #speaker:Phong #sprite:phong_smile

Nghe vui đấy. #speaker:Nam #sprite:nam_talk
Vậy để tao BlockIdea mày đã nhé. #speaker:Nam #sprite:nam_talk
Hai đứa chả biết gì về Blockchain cả. #speaker:Nam #sprite:nam_talk
Thì mày tính hai đứa làm đồ án kiểu gì? #speaker:Nam #sprite:nam_talk
Vừa ngồi học vừa làm à? #speaker:Nam #sprite:nam_bored

... #speaker:Nam #sprite:nam_exhaust
... #speaker:Phong #sprite:phong_exhaust

Để tao lên thư viện kiếm ý tưởng. #speaker:Nam #sprite:nam_talk
Mày ở nhà lên mạng kiếm đê, biết đâu kiếm được ý tưởng hay thì sao? #speaker:Nam #sprite:nam_talk

Oke! Có gì tao gọi lại. #speaker:Phong #sprite:phong_talk

Kay. #speaker:Nam #sprite:nam_talk
-> DONE

// ----------- Tại map1 hallway 1 ----------
=== ch1_Hallway1 ===
{ talked_libarian == false:
    ~ talked_libarian = true
    Em đến tìm kiếm sách gì à? #speaker:Thủ thư #sprite:libarian_talk
    Thư viện có tủ sách nào liên quan đến công nghệ thông tin không ạ? #speaker:Nam #sprite:nam_talk
    Hmm... #speaker:Thủ thư #sprite:libarian_talk
    Em kiểm tra thử dãy sách ở hành lang nha. #speaker:Thủ thư #sprite:libarian_talk
    Vâng, em cảm ơn chị. #speaker:Nam #sprite:nam_talk
    À, nhớ tránh các bạn đang đọc sách với cô lao công, cẩn thận va trúng mấy bạn. #speaker:Thủ thư #sprite:libarian_talk
    ->DONE
- else:
    Có nhiều sách mới nhập về, em xem thử có sách của em không? #speaker:Thủ thư #sprite:libarian_talk
    -> DONE
}
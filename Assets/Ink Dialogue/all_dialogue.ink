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


// ----------- Cutscene đầu ch1 ----------
=== ch1_Cutscene1 ===
#bg:dom
Màn hình máy tính sáng lên trong căn phòng trọ bao bọc trong tĩnh lặng và bóng tối. 
Chỉ có tiếng nhạc game đơn điệu phát ra từ máy tính cùng âm thanh gõ bàn phím lạch cạch của Nam.

Thực tập xong rồi. Môn cũng hết rồi. #speaker:Nam #sprite:nam_thinking
Mọi thứ đáng lẽ phải nhẹ nhõm hơn chứ? #speaker:Nam #sprite:nam_thinking
Vậy mà sao mình lại thấy trống rỗng thế này... #speaker:Nam #sprite:nam_bored

* [Tiếp tục chơi game]
    #bg:black
    Cứ chơi thêm một ván nữa. Rồi lại một ván nữa. 
    Haizzz... Chán thật. #speaker:Nam #sprite:nam_bored #bg:dom
    Cái cảm giác vô định này... #speaker:Nam #sprite:nam_bored
    Mình không thích nó!! #speaker:Nam #sprite:nam_angry
    -> touchgrass

* [Nhìn ra ngoài cửa sổ]
    #bg:dom_pull_curtain
    Sau khi kéo rèm cửa, ánh sáng trải lên khắp phòng một màu vàng nhạt. #bg:dom_pull_curtain
    Mình đang làm gì với đời vậy nhờ?? #speaker:Nam #sprite:nam_thinking
    -> touchgrass

= touchgrass
Không thể cứ lầm lì mãi một chỗ được. #speaker:Nam #sprite:nam_thinking
Ra ngoài đường thư giãn đầu óc thôi. #speaker:Nam #sprite:nam_thinking
#bg:nam_touchgrass
-> DONE
    
// ----------- Tại lobby tương tác với điện thoại ----------
=== ch1_Lobby1 ===
{ interact_phone == false:
    ~ interact_phone = true

    _"Ting!!!!!"_
    "Bạn có một thông báo mới từ Discord."

    "Êy cu, làm <b>đồ án tốt nghiệp</b> với tao ko?" #speaker:Phong 
    "Đằng nào tao với mày cũng xong mấy môn sớm," #speaker:Phong 
    "Thì tại sao hai tụi mình ko làm đồ án sớm luôn chứ nhỉ??" #speaker:Phong

    Uầyyy!!! Nó rủ mình làm đồ án chung này. #speaker:Nam #sprite:nam_surprise
    Cơ mà mình lỡ thất bại thì sao? #speaker:Nam #sprite:nam_thinking
    Nếu mình ko đủ giỏi thì sao? #speaker:Nam #sprite:nam_thinking
    Lỡ mà kéo nó xuống chung với mình thì sao? #speaker:Nam #sprite:nam_thinking

    "Tao quen mày lâu tao mới dám rủ" #speaker:Phong
    "Chứ mấy đứa khác tao không yên tâm. Với lại cũng ngại chết mày ơi!" #speaker:Phong
    "Thế chú có tính làm ko?" #speaker:Phong 

    + [Đồng ý]
        -> decision

    + [Lưỡng lự]
        ... #speaker:Nam #sprite:nam_confused
        Thôi thì đằng nào cũng chả có việc gì làm. #speaker:Nam #sprite:nam_talk
        -> decision

- else:
    Hiện tại bạn có 0 thông báo.
    -> DONE
}

= decision
"Kay" #speaker:Nam #sprite:nam_talk
"Thế thì để tao giúp" #speaker:Nam #sprite:nam_talk
"Tao cũng đang rảnh chán" #speaker:Nam #sprite:nam_talk
"Công việc thế nào hả, cộng sự" #speaker:Nam #sprite:nam_smile
~ accept_invitation = true
-> DONE

=== ch1_Lobby1_LockDoor ===
Khoan nào... #speaker:Nam #sprite:nam_thinking
Hình như mình quên gì đó thì phải... #speaker:Nam #sprite:nam_thinking
Điện thoại mình đâu rồi nhỉ?! #speaker:Nam #sprite:nam_thinking
-> DONE


// ----------- Cutscene sau lobby1 ----------
=== ch1_Cutscene2 ===
#bg:one_week_later 
#bg:friend_dom
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
    Chị thủ thư ơi? #speaker:Nam #sprite:nam_talk
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

// ----------- Trong map1 hallway2 ----------
=== ch1_Hallway2 ===
{ talked_hallway2 == false:
    ~ talked_hallway2 = true
    Cả tủ sách chỉ có 3 cuốn là xài được. #speaker:Nam #sprite:nam_thinking
    Mình nên lấy cuốn nào đây ta? #speaker:Nam #sprite:nam_talk
    -> choose_book

- else:
    Có một cuốn sách bị thiếu trên kệ có lẽ đang nằm trong tay bạn.
    -> DONE
}

= choose_book
{ pick_any_book:
    Tiếp theo là cuốn nào đây? #speaker:Nam #sprite:nam_thinking
}

{ not chose_book1:
    + [300 bài code thanh niên.]
    ~ chose_book1 = true
    ~ pick_any_book = true
    Nội dung hơi chung chung, cuốn này không xài được rồi, haizz... #speaker:Nam #sprite:nam_thinking
    -> choose_book
}

{ not chose_book2:
    + [Sáng tạo trong IT, hãy bùng nổ kiến thức.]
    ~ chose_book2 = true
    ~ pick_any_book = true
    "....Để trở nên sáng tạo trong lập trình, hãy lập trình những thứ mà mình thích và tận hưởng những khó khăn của nó."
    Hmm... #speaker:Nam #sprite:nam_thinking
    Vẫn chưa kiếm ra ý tưởng từ sách này. #speaker:Nam #sprite:nam_thinking
    -> choose_book
}

{ not chose_book3:
    + [Pro Gamer thì phải làm Game!??]
    ~ chose_book3 = true
    Ra đây là sách hướng dẫn lập trình game à!? #speaker:Nam #sprite:nam_thinking
    Cũng thú vị, để mang vể đọc thử. #speaker:Nam #sprite:nam_thinking
    -> after_choose_book
}

= after_choose_book
_"Ring Ring!!!"_
Mày kiếm được gì rồi à? #speaker:Nam #sprite:nam_talk
Tao kiếm được.... #speaker:Phong #sprite:phong_talk
Dark Soup 3 mới ra mắt, chơi luôn không cu. #speaker:Phong #sprite:phong_talk
Nghe nói game lần này giải đố nhiều lắm đấy! #speaker:Phong #sprite:phong_talk
Uầy hay, tao với mày đợi game cũng phải nửa năm rồi đấy!! #speaker:Nam #sprite:nam_talk
Đợi tao về chơi luôn, tao cấm mày chơi trước!! #speaker:Nam #sprite:nam_talk
OKê! #speaker:Phong #sprite:phong_smile
-> DONE

=== ch1_Hallway2_LockDoor ===
Phòng này nhiều sách thật đấy chứ! #speaker:Nam #sprite:nam_talk
Chắc phải có ý tưởng mình cần kiếm trong này. #speaker:Nam #sprite:nam_talk
-> DONE

// ----------- Trong map1 hallway3 ----------
=== ch1_Hallway3 ===
{ talked_hallway3 == false:
    ~ talked_hallway3 = true
    Em kiếm được sách của em chưa? #speaker:Thủ thư #sprite:libarian_talk
    Có rồi chị ơi. #speaker:Nam #sprite:nam_talk
    Em thấy cuốn này cũng hay nên định mang về đọc thử. #speaker:Nam #sprite:nam_talk

    ... #speaker:Thủ thư #sprite:libarian_talk
    Hướng dẫn làm game à, làm chị nhớ đến khoá trước cũng có người làm đồ án game. #speaker: Thủ thư #sprite:libarian_smile
    
    Thật vậy hả chị? #speaker:Nam #sprite:nam_surprise
    
    Ừ. #speaker:Thủ thư #sprite:libarian_talk
    Mấy bạn đó cũng từng quanh quẩn ở khu cuối thư viện để tìm tài liệu thêm. #speaker:Thủ thư #sprite:libarian_talk
     
    ... #speaker:Nam #sprite:nam_thinking
    Vậy từ từ để em xem thêm một vòng thử nha chị. #speaker:Nam #sprite:nam_talk

    Vậy thì chị đánh dấu cuốn này lại cho em trước. #speaker:Thủ thư #sprite:libarian_talk
    Có mà lấy thêm sách nhớ quay lại chỗ chị nhá. #speaker:Thủ thư #sprite:libarian_talk
    À vâng. #speaker:Nam #sprite:nam_talk
    À mà đằng sau có tủ sách tự do ấy! #speaker:Thủ thư #sprite:libarian_talk
    Em có thích cuốn sách gì mang về đọc thì cứ lấy. #speaker: Thủ thư #sprite:libarian_smile
    Mỗi người được mang 1 cuốn về. #speaker:Thủ thư #sprite:libarian_talk
    Còn nếu có sách nào hay thì mang lên chia sẻ cùng mọi người nha. #speaker: Thủ thư #sprite:libarian_smile
    Vâng ạ!#speaker:Nam #sprite:nam_talk
    Em cảm ơn chị! #speaker:Nam #sprite:nam_talk
    -> DONE

- else:
    Nhớ trả sách vào tuần sau nhé. #speaker:Thủ thư #sprite:libarian_talk
    -> DONE
}

=== ch1_Hallway3_LockDoor ===
Em có mượn sách thư viện không em ơi ?? #speaker:Thủ thư #sprite:libarian_talk
Nếu có thì lại đây để chị đóng dấu đã nhé!  #speaker:Thủ thư #sprite:libarian_talk
-> DONE

// ----------- Nhặt được vật phẩm ----------
=== ch1_GetItem ===
Có một câu hỏi giải đố nằm trên kệ sách. #speaker:Nam #sprite:nam_talk
Hmm.... #speaker:Nam #sprite:nam_thinking
Để nghiên cứu sau. #speaker:Nam #sprite:nam_talk
-> END 

// ----------- Tại đích ----------
=== ch1_At_Goal ===
_"Ring Ring!!!"_
Về chưa đấy. #speaker:Phong #sprite:phong_talk
Không về nhanh cẩn thận tao chơi trước đấy nhá. #speaker:Phong #sprite:phong_talk
-> DONE

// ----------- Cutscene sau khi chơi game xong tại phòng trọ ----------
=== ch1_Cutscene3 ===
#bg: dom_friend
Game cuốn thật. #speaker:Nam #sprite:nam_talk
Nhìn đơn giản vậy mà giải đố cũng căng phết. #speaker:Phong #sprite:phong_talk
Ừ. #speaker:Nam #sprite:nam_talk

Cơ mà tao lại thích kiểu puzzle nhanh gọn hơn. #speaker:Phong #sprite:phong_smile
Kiểu mỗi màn giải một cái là xong ấy. #speaker:Phong #sprite:phong_smile

Giống tao. #speaker:Nam #sprite:nam_smile
... #speaker:Nam #sprite:nam_thinking

À mà. #speaker:Nam #sprite:nam_thinking
Hồi chiều tao lên thư viện ấy. #speaker:Nam #sprite:nam_talk
Kiếm được gì không? #speaker:Phong #sprite:phong_talk
Ban đầu toàn sách linh tinh thôi. #speaker:Nam #sprite:nam_talk

{chose_book2:
    Cơ mà nãy tao kiếm được 1 câu cũng hay. #speaker:Nam #sprite:nam_talk
    "Nếu muốn sáng tạo, hãy làm thứ mà mình thật sự thích." #speaker:Nam #sprite:nam_talk
    Ban đầu nó không liên quan tới đồ án nên tao cũng không để tâm lắm. #speaker:Nam #sprite:nam_talk
    Giờ nghĩ lại thì câu đấy nó lại đúng trong trường hợp này. #speaker:Nam #sprite:nam_talk
    ... #speaker:Phong #sprite:phong_thinking
}

Trong đống linh tinh đấy thì tao kiếm được cuốn hướng dẫn lập trình game. #speaker:Nam #sprite:nam_talk
Thấy thú vị thì tao mang về xem thử. #speaker:Nam #sprite:nam_talk

À mà nhắc mới nhớ. #speaker:Nam #sprite:nam_surprise
Hồi nãy chị thủ thư kể khoá trước cũng từng có nhóm làm đồ án game rồi. #speaker:Nam #sprite:nam_talk
Nghe nói làm cũng ổn phết. #speaker:Nam #sprite:nam_talk
Thật à! #speaker:Phong #sprite:phong_surprise

Ừ. #speaker:Nam #sprite:nam_smile
Mà nghĩ lại thì... #speaker:Nam #sprite:nam_thinking
Cả tao với mày đều thích game mà nhỉ. #speaker:Nam #sprite:nam_talk

Ừ thì... thích thật. #speaker:Phong #sprite:phong_smile
Hai đứa mình cũng cày nát bao nhiêu game rồi còn gì. #speaker:Phong #sprite:phong_talk

Ừ. #speaker:Nam #sprite:nam_smile
Thế nên tao mới nghĩ... #speaker:Nam #sprite:nam_talk

Hay là... #speaker:Nam #sprite:nam_thinking
ĐỒ ÁN TỐT NGHIỆP, làm con puzzle game như cái cách tao với mày muốn. #speaker:Nam #sprite:nam_talk
Tao thấy nó thú vị đấy chứ. #speaker:Nam #sprite:nam_talk
... #speaker:Phong #sprite:phong_surprise
... #speaker:Phong #sprite:phong_thinking
... #speaker:Phong #sprite:phong_surprise
Tao cũng thấy ý tưởng thú vị đấy. #speaker:Phong #sprite:phong_smile
Biết đâu sau này có người khác lại chơi game của tụi mình. #speaker:Phong #sprite:phong_smile
Thế bắt đầu từ đâu đây? #speaker:Phong #sprite:phong_talk

Từ cái đơn giản nhất thôi. #speaker:Nam #sprite:nam_smile
Nghĩ gameplay trước. #speaker:Nam #sprite:nam_talk
Sau đó mới tới map với cốt truyện. #speaker:Nam #sprite:nam_talk

... #speaker:Nam #sprite:nam_smile
... #speaker:Phong #sprite:phong_smile
// TODO: dan sang chapter 2
-> DONE


// =============================================
// CHAPTER 2: NHỮNG TRỞ NGẠI BẤT NGỜ
// =============================================

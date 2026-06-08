// =============================================
// CHAPTER 1: KHI CÒN CHƯA BIẾT ĐI VỀ ĐÂU
// =============================================

// GLOBAL
// VAR ch1_lobby_phone_StartedQuest = false
VAR ch1_lobby_phone_AcceptedInvite = false
VAR ch1_cutscene1_Discussed = false

// INTERACTIONS
VAR ch1_lobby_door_Interacted = false
VAR ch1_hallway2_bookshelf_PickedBooks= false
VAR ch1_hallway2_monitor_hasInteracted = false
VAR ch1_hallway2_paper_hasRead = false
VAR item_voucher = false

// BOOKs CHOICE
VAR ch1_hallway2_bookshelf_book1 = false
VAR ch1_hallway2_bookshelf_book2 = false
VAR ch1_hallway2_bookshelf_book3 = false

// NPCs
VAR ch1_hallway1_librarian = false
VAR ch1_hallway2_bookshelf = false
VAR ch1_hallway3_librarian = false

// LOCK DOORs
// VAR ch1_lobby_door = 0
VAR ch1_hallway2_door = 0
VAR ch1_hallway3_door = 0

// ----------- Cutscene đầu ch1 ----------
=== ch1_cutscene1 ===
#sfx:begin #cg:chapter1,3,full 
#cg:black,4,full 
// Gợi ý: lofi piano chậm, đơn điệu, không có percussion

Ánh sáng xanh nhạt từ màn hình hắt lên căn phòng trọ nhỏ.
Tiếng bàn phím lách cách vang lên. Rồi dừng lại.
Trên màn hình là dòng chữ: <b>VICTORY</b>
Nam tháo tai nghe xuống.
...
#sfx:camera_tick
#bgm:vn_theme
Góc phải màn hình hiện thông báo:
<i>Hoàn thành kỳ thực tập.</i>
<i>Đã kết thúc học phần cuối cùng.</i>
...
Đáng lẽ đây phải là cảm giác nhẹ nhõm chứ !!! #speaker:Nam #sprite:nam_thinking
#bg:dom

Nam tựa lưng vào ghế.
Trần nhà vẫn như mọi ngày.
Căn phòng vẫn như mọi ngày.
...
"Giờ thì làm gì tiếp đây..." #speaker:Nam #sprite:nam_bored
* [Tiếp tục chơi game]
    #cg:black,2,full #sfx:camera_tick
    Màn hình sáng lên. Âm thanh chiến thắng vang lên lần thứ ba.
    #cg:black,2,full #sfx:camera_tick
    Nam không cảm thấy gì cả.
    #cg:black,2,full #sfx:camera_tick
    #bg:dom
    ...
    Ngón tay cậu dừng lại trên chuột.
    "Không. Không ổn." #speaker:Nam #sprite:nam_angry
    -> touchgrass

* [Nhìn ra ngoài cửa sổ]
    #bg:dom_pull_curtain
    Rèm cửa được kéo sang một bên.
    Ánh sáng từ cửa sổ trải dài lên khuôn mặt thẩn thờ của Nam.
    Nam đứng yên nhìn ra ngoài.
    ...
    -> touchgrass

= touchgrass
Không thể tiếp tục ngồi trong phòng như vậy được. #speaker:Nam #sprite:nam_bored
Nam với lấy áo khoác trên móc cửa.
-> DONE
    
// ----------- Tại lobby tương tác với điện thoại ----------
=== lobby1_phone ===
{ch1_lobby_door_Interacted == false:
    -> phone_date
- else:
    { phone_call == 0:
        -> phone_call
    - else:
        -> phone_notification_none
    }
}

= phone_date
<i>Ngày 28 tháng 2 - 16:32.
-> DONE


= phone_call
Màn hình điện thoại sáng lên.
<i>"Bạn có một cuộc gọi từ Discord."</i>
Nam nhìn tên hiện trên màn hình.
<i>Phong.</i>
...
"Alo, Phong đấy à." #speaker:Nam #sprite:nam_surprise
<i>"Helo anh bạn."</i> #speaker:Phong
<i>"Làm <b>đồ án tốt nghiệp</b> chung với tôi không?"</i> #speaker:Phong
Nam dừng lại.
...
<i>"Đằng nào tụi mình cũng xong mấy môn sớm."</i> #speaker:Phong
<i>"Làm sớm luôn thì sao?"</i> #speaker:Phong
Ngón tay cậu khựng lại.
Trong đầu thoáng qua những bản báo cáo thực tập. Những lần sửa bài từ khuya đến sáng.
...
<i>"Tôi nghĩ rồi mới rủ ông đấy."</i> #speaker:Phong
<i>"Nếu là ông thì tôi yên tâm."</i> #speaker:Phong
<i>"Này."</i> #speaker:Phong
<i>"Ông vẫn còn nghe chứ?"</i> #speaker:Phong
"Ừ." #speaker:Nam #sprite:nam_talk
<i>"Vậy làm không?"</i> #speaker:Phong

-> selection

= selection
+ [Đồng ý]
    -> decision
* [Lưỡng lự]
    Nam không trả lời ngay.
    ...
    ...
    Thôi thì đằng nào cũng chả có việc gì làm. #speaker:Nam #sprite:nam_talk
    Phong cũng đã có ý rủ mình rồi, giờ mà chán đời nữa đến khi nào mình mới thay đổi được. #speaker:Nam #sprite:nam_talk
    -> selection

= decision
"Được." #speaker:Nam #sprite:nam_talk
"Tôi làm." #speaker:Nam #sprite:nam_talk
<i>"Tuyệt vời lắm người anh em, có ông làm đồ án chung kiểu gì cũng vui hết." #speaker:Phong

~ ch1_lobby_phone_AcceptedInvite = true
-> DONE

= phone_notification_none
Hiện tại bạn có 0 thông báo.
-> DONE


=== lobby1_door ===
{ lobby1_door:
- 1:
    ~ ch1_lobby_door_Interacted = true
    Có người đang gọi cho bạn.
    -> DONE
- 2:
    Điện thoại của bạn đang reo!!!
    Mình để nó ở đâu rồi nhỉ?! #speaker:Nam #sprite:nam_talk
    -> DONE  
- 3:
    Giờ nay ai gọi đấy? #speaker:Nam #sprite:nam_talk
    Mà cái điện thoại mình giấu ở đâu rồi ?? #speaker:Nam #sprite:nam_talk
    -> DONE
- else:
    <b>CÁI ĐIỆN THOẠIII.........!!</b> #speaker:Nam #sprite:nam_angry
    -> DONE
}


// ----------- Cutscene sau lobby1 ----------
=== ch1_cutscene2 ===
#sfx:calendar_flip #cg:one_week_later,3,full 
// Gợi ý: piano + ukulele nhẹ, hơi tò mò, cảm giác bàn bạc thoải mái
#bgm:vn_theme
#bg:friend_dom"
Đồ án làm thử AI không? #speaker:Phong #sprite:phong_talk 
Hả? Làm thử? Ông tính làm thử AI á? #speaker:Nam #sprite:nam_talk
Làm AI chắc cũng đơn giản mà nhỉ?  #speaker:Phong #sprite:phong_talk
Từ lọc cả ngàn ảnh, đánh dấu vật thể, gắn tag cho từng ảnh thì chắc đơn giản với mỗi ông. #speaker:Nam #sprite:nam_talk
Chưa tính đến code hay ý tưởng đề tài phải thật sáng tạo thì ông nhắm 2 đứa làm nổi không? #speaker:Nam #sprite:nam_talk
... #speaker:Phong #sprite:phong_thinking
...Ông nói đúng, còn ý tưởng nào khác không? #speaker:Phong #sprite:phong_talk

Thế sao hai đứa không thử làm Web? #speaker:Nam #sprite:nam_thinking
Ờ, rồi làm giống mấy chục nhóm khác. #speaker:Phong #sprite:phong_talk
Xong mang lên hội đồng so xem ai đẹp hơn à? #speaker:Phong #sprite:phong_talk
Chán lắm, đổi món đi!! #speaker:Phong #sprite:phong_bored

Thế Blockchain thì sao? #speaker:Phong #sprite:phong_talk
Lỡ mà thành công thì hai thằng tung luôn coin ra thị trường luôn. #speaker:Phong #sprite:phong_smile
Vừa có đồ án tốt nghiệp vừa có dự án khởi nghiệp. #speaker:Phong #sprite:phong_smile
Hehe #speaker:Phong #sprite:phong_smile

Nghe vui đấy. #speaker:Nam #sprite:nam_talk
Vậy để mở đầu dự án Blockchain thì tôi BlockIdea ông đã nhé. #speaker:Nam #sprite:nam_talk
Hai đứa chả biết gì về Blockchain cả. #speaker:Nam #sprite:nam_talk
Thì ông tính hai đứa làm đồ án kiểu gì? #speaker:Nam #sprite:nam_talk
Vừa ngồi học vừa làm à? #speaker:Nam #sprite:nam_bored

... #speaker:Nam #sprite:nam_thinking
... #speaker:Phong #sprite:phong_thinking

... #speaker:Nam #sprite:nam_exhaust
... #speaker:Phong #sprite:phong_exhaust

Để tôi lên thư viện kiếm ý tưởng. #speaker:Nam #sprite:nam_talk
Ông ở nhà lên mạng kiếm đê, biết đâu kiếm được ý tưởng hay thì sao? #speaker:Nam #sprite:nam_talk
Oke! Kiếm được gì hay tôi báo cho ông. #speaker:Phong #sprite:phong_talk
Kay. #speaker:Nam #sprite:nam_talk
-> DONE


// ----------- Tại map1 hallway 1 ----------
=== hallway1_Librarian ===
{ ch1_hallway1_librarian == false:
    ~ ch1_hallway1_librarian = true
    Có gì không em??... #speaker:Thủ thư #sprite:libarian_talk
    Cho em hỏi thư viện có tủ sách nào liên quan đến công nghệ thông tin không ạ? #speaker:Nam #sprite:nam_talk
    Hmm... #speaker:Thủ thư #sprite:libarian_talk
    Người thủ thư suy nghĩ một chút.
    Em kiểm tra thử dãy sách ở hành lang thứ hai nha. #speaker:Thủ thư #sprite:libarian_talk
    Vâng, em cảm ơn chị. #speaker:Nam #sprite:nam_talk
    À, nhớ tránh các bạn đang đọc sách, cẩn thận va trúng mấy bạn đấy nhá. #speaker:Thủ thư #sprite:libarian_talk
    ->DONE
- else:
    Có nhiều sách mới nhập về, em xem thử có sách của em không? #speaker:Thủ thư #sprite:libarian_talk
    -> DONE
}

// ----------- Trong map1 hallway2 ----------
=== hallway2_Door ===
{ ch1_hallway2_door:
- 0:
    ~ ch1_hallway2_door = 1
    Phòng đọc sách.
    Nhiều hơn Nam nghĩ.
    "Chắc có gì đó trong này." #speaker:Nam #sprite:nam_thinking
    -> DONE
- 1:
    ~ch1_hallway2_door = 2
    Nam đứng nhìn vào phòng lần nữa.
    Không thể về tay không được. #speaker:Nam #sprite:nam_talk
    Trong phòng này hẳn có <i>ý tưởng</i> mình có thể sử dụng. #speaker:Nam #sprite:nam_talk
    -> DONE
- else:
    <b>KIẾM CÁI Ý TƯỞNGGGGG.....!!!</b>  #speaker:Nam #sprite:nam_angry
    -> DONE
}


=== hallway2_Bookshelf ===
{ ch1_hallway2_bookshelf == false:
    ~ ch1_hallway2_bookshelf = true
    Mùi giấy cũ thoang thoảng trong không khí.
    Nam đứng im vài giây.
    Hàng trăm gáy sách xếp sát nhau.
    Hàng trăm lựa chọn.
    ...
    "Ít nhất cũng phải mang về được một ý tưởng." #speaker:Nam #sprite:nam_thinking
    -> choose_book

- else:
    Có một cuốn sách bị thiếu trên kệ có lẽ đang nằm trong tay bạn.
    -> DONE
}

= choose_book
{ ch1_hallway2_bookshelf_PickedBooks:
    Nam nhìn sang các cuốn còn lại. #speaker:Nam #sprite:nam_thinking
}

{ not ch1_hallway2_bookshelf_book1:
    + [300 bài code thanh niên.]
    ~ ch1_hallway2_bookshelf_book1 = true
    ~ ch1_hallway2_bookshelf_PickedBooks= true
    Cậu lật qua vài trang.
    ...
    ...
    ...
    "Không phải cái này." #speaker:Nam #sprite:nam_thinking
    Nam đặt cuốn sách trở lại kệ.
    -> choose_book
}

{ not ch1_hallway2_bookshelf_book2:
    + [Ý kiến là chính. Ý thích là 10!!!]
    ~ ch1_hallway2_bookshelf_book2 = true
    ~ ch1_hallway2_bookshelf_PickedBooks= true
    ...
    ...
    <i>"....Để trở nên sáng tạo trong lập trình, hãy lập trình những thứ mà mình thích và tận hưởng những khó khăn của nó."</i>
    Nam đọc lại dòng đó một lần nữa.
    ...
    "Chưa phải." #speaker:Nam #sprite:nam_thinking
    -> choose_book
}

{ not ch1_hallway2_bookshelf_book3:
    + [Pro Gamer thì phải làm sao!??]
    ~ ch1_hallway2_bookshelf_book3 = true
    ...
    ...
    Nam lật bìa sách.
    Ra đây là sách hướng dẫn lập trình game à!? #speaker:Nam #sprite:nam_talk
    Cũng thú vị, để mang vể đọc thử. #speaker:Nam #sprite:nam_thinking
    -> after_choose_book
}

= after_choose_book
_"Ring Ring!!!"_
Điện thoại rung trong túi.
Nam nhìn màn hình.
<i>Phong.</i>

"Sao rồi?" #speaker:Nam #sprite:nam_talk
<i>"Tôi kiếm được rồi."</i> #speaker:Phong
<i>"Tôi kiếm được...."</i> #speaker:Phong #sprite:phong_talk
<i>"Dark Soup 3 mới ra mắt, chơi luôn không anh bạn."</i> #speaker:Phong #sprite:phong_talk
Nghe nói game lần này giải đố nhiều lắm đấy! #speaker:Phong #sprite:phong_talk
...
Nam nhìn cuốn sách vừa cầm trên tay.
"Tôi về liền." #speaker:Nam #sprite:nam_talk
"Ai thua bao kèo đi ăn." #speaker:Nam #sprite:nam_talk
<i>"Oke!"</i> #speaker:Phong
-> DONE

=== hallway2_Monitor ===
{ ch1_hallway2_monitor_hasInteracted == false:
    ~ ch1_hallway2_monitor_hasInteracted = true
    Nam ngồi xuống trước chiếc máy tính công cộng.
    Màn hình phản chiếu khuôn mặt chán nản của Nam.
    ...
    Chỉ vài phút sau, những video đề xuất đầy màu sắc đã chen kín góc màn hình.
    ...
    ...
    Màn hình máy tính đột nhiên xuất hiện dòng chữ.
    "<color=Red>YOU DELAYED</color>".
    Nam khẽ chớp mắt.
    "Mình đang làm cái gì vậy..." #speaker:Nam #sprite:nam_thinking
    Mình cần tập trung tìm kiếm. #speaker:Nam #sprite:nam_thinking
    -> DONE
- else:
     "<color=Red>YOU DELAYED</color>".
    -> DONE
}
-> DONE

=== hallway2_Paper ===
{ ch1_hallway2_paper_hasRead == false:
    ~ ch1_hallway2_paper_hasRead = true

    Một tờ giấy nhỏ nằm trên bàn.
    "<b>GitGud</b>".
    ...
    Nam nhìn tờ giấy một lúc.
    Rồi cậu đứng dậy. #speaker:Nam #sprite:nam_smile
    -> DONE
- else: 
    Nam lật mặt sau tờ giấy.
    "Coder <s>May</s> Cry".
    -> DONE
}

// ----------- Trong map1 hallway3 ----------
=== hallway3_Librarian ===
{ ch1_hallway3_librarian == false:
    ~ ch1_hallway3_librarian = true
    Em kiếm được sách của em chưa? #speaker:Thủ thư #sprite:libarian_talk
    Có rồi chị ơi. #speaker:Nam #sprite:nam_talk
    Em thấy cuốn này cũng hay nên định mang về đọc thử. #speaker:Nam #sprite:nam_talk

    Người thủ thư nhìn bìa sách. #speaker:Thủ thư #sprite:libarian_thinking
    ...
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
    Em có thích cuốn sách gì mang về đọc thì lấy. #speaker: Thủ thư #sprite:libarian_smile
    Mỗi người được mang 1 cuốn về. #speaker:Thủ thư #sprite:libarian_talk
    Còn nếu có sách nào hay thì mang lên chia sẻ cùng mọi người nha. #speaker: Thủ thư #sprite:libarian_smile
    Vâng ạ!#speaker:Nam #sprite:nam_talk
    Em cảm ơn chị! #speaker:Nam #sprite:nam_talk
    -> DONE

- else:
    Nhớ trả sách vào tuần sau nhé. #speaker:Thủ thư #sprite:libarian_talk
    -> DONE
}

=== hallway3_Door ===
{ch1_hallway3_door:
- 0:
    ~ ch1_hallway3_door = 1
    Em có mượn sách thư viện không em ơi ?? #speaker:Thủ thư #sprite:libarian_talk
    Nếu có thì lại đây để chị đóng dấu đã nhé!  #speaker:Thủ thư #sprite:libarian_talk
    -> DONE
- else: 
    ~ ch1_hallway3_door = 2
    Đợi chị đóng dấu sách đã em trai!! #speaker:Thủ thư #sprite:libarian_talk
    -> DONE
}


// ----------- Nhặt được vật phẩm ----------
=== hallway3_GetItem ===
~ item_voucher = true
Có gì đó nằm im dưới đáy kệ sách.
Nam cúi nhặt lên.
...
Một tờ phiếu nhỏ. Chữ in đã hơi mờ.
-> DONE

// ----------- Tại đích ----------
=== hallway3_AtGoal ===
_"Ring Ring!!!"_
<i>"Về chưa đấy."</i> #speaker:Phong
<i>"Không về nhanh cẩn thận tôi chơi trước đấy nhá."</i> #speaker:Phong
-> DONE


// ----------- Cutscene sau khi chơi game xong tại phòng trọ ----------
=== ch1_cutscene3 ===
#cg: timeskip_evening,5,full #sfx: clock_ticking
#bgm:vn_theme
#bg:dom_friend

Màn hình tắt.
Tiêu đề trò chơi hiện ra.
<b>GAME CLEAR.</b>

Cuốn thật. #speaker:Nam #sprite:nam_smile
Nhìn đơn giản mà căng phết. #speaker:Phong #sprite:phong_talk
Ừ. #speaker:Nam #sprite:nam_talk

Phong đặt tay cầm xuống bàn.
Tôi lại thích kiểu puzzle nhanh gọn hơn. #speaker:Phong #sprite:phong_smile
Mỗi màn một cái là xong. #speaker:Phong #sprite:phong_smile

Nam gật đầu nhẹ.
Giống tôi. #speaker:Nam #sprite:nam_smile

...

À mà. #speaker:Nam #sprite:nam_thinking
Phong nhìn sang.
Hôm nay tôi lên thư viện. #speaker:Nam #sprite:nam_talk
Kiếm được gì không? #speaker:Phong #sprite:phong_talk
Ban đầu toàn sách linh tinh. #speaker:Nam #sprite:nam_talk

{ch1_hallway2_bookshelf_book2:
    Cơ mà nãy tui kiếm được 1 câu cũng hay. #speaker:Nam #sprite:nam_talk
    <i>"Nếu muốn sáng tạo, hãy làm thứ mà mình thật sự thích."</i> #speaker:Nam #sprite:nam_talk
    Ban đầu nó không liên quan tới đồ án nên tôi cũng không để tâm lắm. #speaker:Nam #sprite:nam_talk
    Giờ nghĩ lại thì câu đấy nó lại đúng... #speaker:Nam #sprite:nam_talk
    ... #speaker:Phong #sprite:phong_thinking
}

Trong đống linh tinh đấy thì tui kiếm được cuốn hướng dẫn lập trình game. #speaker:Nam #sprite:nam_talk
Thấy thú vị thì tui mang về xem thử. #speaker:Nam #sprite:nam_talk

À mà nhắc mới nhớ. #speaker:Nam #sprite:nam_surprise
Chị thủ thư kể khoá trước cũng có nhóm làm đồ án game. #speaker:Nam #sprite:nam_talk
Nghe nói làm ổn phết. #speaker:Nam #sprite:nam_talk
Thật không? #speaker:Phong #sprite:phong_surprise

Nam không trả lời ngay.
Cậu nhìn màn hình tiêu đề của trò chơi vừa hoàn thành.
Những căn phòng khóa kín.
Những câu đố.
Những đáp án được ghép lại từng chút một.

//chuyển sang bgm_warm_discovery
#bgm:vn_theme
Tụi mình thích game lâu rồi nhỉ. #speaker:Nam #sprite:nam_talk
Ừ thì... #speaker:Phong #sprite:phong_smile
Cày nát bao nhiêu game rồi còn gì. #speaker:Phong #sprite:phong_talk
Nếu... Nếu tụi mình thử làm một trò chơi thì sao?" #speaker:Nam #sprite:nam_talk

Game? #speaker:Phong #sprite:phong_surprise

Ừ. #speaker:Nam #sprite:nam_talk
Puzzle. #speaker:Nam #sprite:nam_talk
Theo kiểu tụi mình thích. #speaker:Nam #sprite:nam_talk

Căn phòng im lặng vài giây.

... #speaker:Phong #sprite:phong_thinking
... #speaker:Phong #sprite:phong_surprise

Nghe được đấy. #speaker:Phong #sprite:phong_smile
Biết đâu sau này có người chơi game của tụi mình. #speaker:Phong #sprite:phong_smile

Nam nhìn cuốn sách lập trình game đặt trên bàn.
Quyển sách được mang về chỉ vì tò mò.
Không hiểu sao.
Mọi thứ bỗng nhiên kết nối lại với nhau.

Bắt đầu từ đâu? #speaker:Phong #sprite:phong_talk

Gameplay trước. #speaker:Nam #sprite:nam_smile
Sau đó mới tới map với cốt truyện. #speaker:Nam #sprite:nam_talk

... #speaker:Nam #sprite:nam_smile
... #speaker:Phong #sprite:phong_smile
-> DONE
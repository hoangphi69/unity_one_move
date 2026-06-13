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
#sfx:collect #cg:chapter1,2,full 
#cg:black,2,full

Ánh sáng xanh nhạt từ màn hình hắt lên căn phòng trọ nhỏ. #bg:dom
Tiếng bàn phím lách cách vang lên. Rồi dừng lại.
Trên màn hình là dòng chữ: <b>VICTORY</b>
Nam tháo tai nghe xuống.
...
#sfx:camera_snap
#bgm:vn_theme
Góc phải màn hình hiện thông báo: #bg:blur
<i>Hoàn thành kỳ thực tập.</i>
<i>Đã kết thúc học phần cuối cùng.</i>
...
#bg:blur
Đáng lẽ đây phải là cảm giác nhẹ nhõm chứ !!! #speaker:Nam #sprite:nam_angry
#bg:dom
Nam tựa lưng vào ghế.
Trần nhà vẫn như mọi ngày.
Căn phòng vẫn như mọi ngày.
...
"Giờ thì làm gì tiếp đây..." #speaker:Nam #sprite:nam_bored
* [Tiếp tục chơi game]
    #sfx:camera_snap #cg:black,1,full 
    #bg:dom
    Màn hình sáng lên. Âm thanh chiến thắng vang lên lần thứ ba.
    #sfx:camera_snap #cg:black,1,full 
    Nhưng tâm trạng Nam vẫn chùng xuống.
    #sfx:camera_snap #cg:black,1,full 
    #bg:dom
    ...
    Ngón tay cậu dừng lại trên bàn phím.
    #bg:blur
    Cứ thế này thì không ổn rồi!!! #speaker:Nam #sprite:nam_angry
    -> touchgrass

* [Nhìn ra ngoài cửa sổ]
    #bg:dom
    Rèm cửa được kéo sang một bên.
    Ánh sáng từ cửa sổ trải dài lên khuôn mặt thẩn thờ của Nam.
    Nam đứng yên nhìn ra ngoài.
    ...
    -> touchgrass

= touchgrass
#bg:dom
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

= phone_notification_none
Hiện tại bạn có 0 thông báo.
-> DONE

= phone_call
Màn hình điện thoại sáng lên.
<i>"Bạn có một cuộc gọi từ Zola."</i>
Nam nhìn tên hiện trên màn hình.
<i>Phong.</i>
...
"Alo, Phong đấy à." #speaker:Nam #sprite:nam_surprise
<i>"Helo bro."</i> #speaker:Phong
<i>"Làm <b>đồ án tốt nghiệp</b> chung với tôi không?"</i> #speaker:Phong
Nam bỗng dừng lại một nhịp.
...
<i>"Đằng nào tụi mình cũng xong môn học sớm."</i> #speaker:Phong
<i>"Làm đồ án sớm luôn thì sao?"</i> #speaker:Phong
Ngón tay cậu khựng lại.
Trong đầu Nam thoáng qua những bản báo cáo thực tập. Những lần sửa bài từ khuya đến sáng.
...
<i>"Tôi nghĩ rồi mới rủ cậu đấy."</i> #speaker:Phong
<i>"Nếu là cậu thì tôi yên tâm."</i> #speaker:Phong
<i>"Này."</i> #speaker:Phong
<i>"Cậu vẫn còn nghe chứ?"</i> #speaker:Phong
"Tôi đây." #speaker:Nam #sprite:nam_talk1
<i>"Vậy ý cậu như nào?"</i> #speaker:Phong

-> selection

= selection
+ [Đồng ý]
    -> decision
* [Lưỡng lự]
    ...
    ...
    Nam không trả lời ngay.
    Cậu nhìn lại căn phòng.
    Màn hình bên kia vẫn còn sáng dòng chữ <b>VICTORY</b>.
    Nam suy nghĩ lại về thời gian mệt mỏi trong kỳ thực tập cũng như thời gian rảnh rỗi chán nản sau khi chấm dứt kỳ thực tập của Nam.
    ... #speaker:Nam #sprite:nam_thinking
    -> selection

= decision
Được. #speaker:Nam #sprite:nam_talk1
Tôi với cậu cùng làm thì có gì khó chứ!! #speaker:Nam #sprite:nam_talk1
<i>"Tuyệt vời lắm người anh em, có cậu làm đồ án chung kiểu gì cũng vui hết." #speaker:Phong

~ ch1_lobby_phone_AcceptedInvite = true
-> DONE



=== lobby1_door ===
~ ch1_lobby_door_Interacted = true
{ lobby1_door:
- 1:
    Có người đang gọi cho bạn.
    -> DONE
- 2:
    Điện thoại của bạn đang reo.
    Mình để nó ở đâu rồi nhỉ?! #speaker:Nam #sprite:nam_talk2
    -> DONE  
- 3:
    Giờ nay ai gọi đấy? #speaker:Nam #sprite:nam_talk2
    Mà cái điện thoại mình giấu ở đâu rồi?? #speaker:Nam #sprite:nam_talk1
    -> DONE
- else:
    <b>CÁI ĐIỆN THOẠIII.........!!</b> #speaker:Nam #sprite:nam_angry
    -> DONE
}


// ----------- Cutscene sau lobby1 ----------
=== ch1_cutscene2 ===
#cg:black,0.2,full #sfx:calendar_flip
#cg:calender_flip,0.2,full 
#cg:dom_friend_morning,0.2,full
#bgm:vn_theme
#bg:dom_friend_morning
Một tuần trôi qua.
Nam và Phong ngồi trước hai cái màn hình.
Giữa hai người là một tờ giấy, vài ý tưởng được gạch đầu dòng.
Nhưng không cái nào có vẻ đúng.


Đồ án làm thử AI không? #speaker:Phong #sprite:phong_talk2

Hả? Làm thử? Cậu tính làm thử AI á? #speaker:Nam #sprite:nam_talk2
Làm AI chắc cũng đơn giản mà nhỉ?  #speaker:Phong #sprite:phong_talk1
Từ lọc cả ngàn ảnh, đánh dấu vật thể, gắn tag cho từng ảnh thì chắc đơn giản với mỗi cậu. #speaker:Nam #sprite:nam_talk2
Chưa tính đến code hay ý tưởng đề tài phải thật sáng tạo thì cậu nhắm 2 đứa làm nổi không? #speaker:Nam #sprite:nam_talk2
...
Cậu nói đúng. #speaker:Phong #sprite:phong_bored
Còn ý tưởng nào khác không? #speaker:Phong #sprite:phong_bored

Thế sao hai đứa không thử làm Web? #speaker:Nam #sprite:nam_idea
Ờ, rồi làm giống mấy chục nhóm khác. #speaker:Phong #sprite:phong_bored
Xong mang lên hội đồng so xem ai đẹp hơn à? #speaker:Phong #sprite:phong_angry
Chán lắm, đổi món đi!! #speaker:Phong #sprite:phong_bored


Thế Blockchain thì sao? #speaker:Phong #sprite:phong_thinking1
Lỡ mà thành công thì hai thằng tung luôn coin ra thị trường luôn. #speaker:Phong #sprite:phong_smile
Vừa có đồ án tốt nghiệp vừa có dự án khởi nghiệp. #speaker:Phong #sprite:phong_smile
Hehe #speaker:Phong #sprite:phong_excited
Nghe vui đấy. #speaker:Nam #sprite:nam_smile
Vậy để mở đầu dự án Blockchain thì tôi BlockIdea cậu đã nhé. #speaker:Nam #sprite:nam_confused
Hai đứa chả biết gì về Blockchain cả. #speaker:Nam #sprite:nam_talk2
Thì cậu tính hai đứa làm đồ án kiểu gì? #speaker:Nam #sprite:nam_talk2
Vừa ngồi học vừa làm à? #speaker:Nam #sprite:nam_silent
Thế phải như nào mới hài lòng được cậu. #speaker:Phong #sprite:phong_angry
Ai mà biết được, phải như nào phù hợp báo cáo hội đồng mà còn vừa sức với anh em mình nữa chứ. #speaker:Nam #sprite:nam_angry
...
...

Sau một thời gian dài tranh cãi, khi cả Nam và Phong biết rằng ngôn từ trở nên bất lực.
Nam và Phong đều quyết định giảng hoà bằng phương pháp... im lặng.
... #speaker:Nam #sprite:nam_silent
... #speaker:Phong #sprite:phong_exhaust

Cả hai không nói gì.
Tờ giấy vẫn như cũ.

...
...
Không được rồi, cứ thế này không ổn. #speaker:Phong #sprite:phong_exhaust
Chả có đề tài nào vừa ý anh em mình. #speaker:Phong #sprite:phong_bored
Nếu vậy thì đi kiếm đề tài thôi. #speaker:Nam #sprite:nam_talk2
Để tôi lên thư viện kiếm ý tưởng. #speaker:Nam #sprite:nam_talk1
Cậu ở nhà lên mạng kiếm đê, biết đâu kiếm được ý tưởng hay thì sao? #speaker:Nam #sprite:nam_talk1
Oke! Kiếm được gì hay tôi báo cho cậu. #speaker:Phong #sprite:phong_thinking2
Kay. #speaker:Nam #sprite:nam_talk1
-> DONE


// ----------- Tại map1 hallway 1 ----------
=== hallway1_librarian ===
{ ch1_hallway1_librarian == false:
    ~ ch1_hallway1_librarian = true
    Chị có thể giúp gì cho em?? #speaker:Thủ thư
    Cho em hỏi thư viện có tủ sách nào liên quan đến công nghệ thông tin không ạ? #speaker:Nam #sprite:nam_talk1
    Hmm... #speaker:Thủ thư
    Thủ thư suy nghĩ một chút.
    Em kiểm tra thử dãy sách ở phòng <b>Tổng hợp</b> nha. #speaker:Thủ thư
    Vâng, em cảm ơn chị. #speaker:Nam #sprite:nam_talk1
    À, nhớ tránh các bạn đang đọc sách, cẩn thận va trúng mấy bạn đấy nhá. #speaker:Thủ thư
    ->DONE
- else:
    Có nhiều sách mới nhập về, em xem thử có sách của em không? #speaker:Thủ thư
    -> DONE
}

// ----------- Trong map1 hallway2 ----------
=== hallway2_door ===
{ ch1_hallway2_door:
- 0:
    ~ ch1_hallway2_door = 1
    Phòng đọc sách.
    Nhiều hơn Nam nghĩ.
    "Chắc có gì đó trong này." #speaker:Nam #sprite:nam_talk2
    -> DONE
- 1:
    ~ch1_hallway2_door = 2
    Nam đứng nhìn vào phòng lần nữa.
    Không thể về tay không được. #speaker:Nam #sprite:nam_angry
    Trong phòng này hẳn có <i>ý tưởng</i> mình có thể sử dụng. #speaker:Nam #sprite:nam_angry
    -> DONE
- else:
    <b>KIẾM CÁI Ý TƯỞNGGGGG.....!!!</b>  #speaker:Nam #sprite:nam_angry
    -> DONE
}


=== hallway2_bookshelf ===
{ ch1_hallway2_bookshelf == false:
    ~ ch1_hallway2_bookshelf = true
    Mùi giấy cũ thoang thoảng trong không khí.
    Nam đứng im vài giây.
    Hàng trăm gáy sách xếp sát nhau.
    Hàng trăm lựa chọn.
    ...
    Ít nhất cũng phải mang về được một ý tưởng. #speaker:Nam #sprite:nam_angry
    -> choose_book

- else:
    Có một cuốn sách bị thiếu trên kệ có lẽ đang nằm trong tay bạn.
    -> DONE
}

= choose_book
{ ch1_hallway2_bookshelf_PickedBooks:
    Nam nhìn sang các cuốn còn lại. #speaker:Nam #sprite:nam_talk2
}

{ not ch1_hallway2_bookshelf_book1:
    + [300 bài code thanh niên.]
    ~ ch1_hallway2_bookshelf_book1 = true
    ~ ch1_hallway2_bookshelf_PickedBooks= true
    Cậu lật qua vài trang.
    ...
    ...
    ...
    Không phải cái này. #speaker:Nam #sprite:nam_confused
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
    Chưa phải. #speaker:Nam #sprite:nam_talk2
    -> choose_book
}

{ not ch1_hallway2_bookshelf_book3:
    + [Pro Gamer thì phải làm sao!??]
    ~ ch1_hallway2_bookshelf_book3 = true
    ...
    ...
    Nam lật bìa sách.
    Ra đây là sách hướng dẫn lập trình game à!? #speaker:Nam #sprite:nam_surprise
    Cũng thú vị, để mang vể đọc thử. #speaker:Nam #sprite:nam_thinking
    -> after_choose_book
}

= after_choose_book
"Reng Reng!!!"
Điện thoại rung trong túi.
Nam nhìn màn hình.
<i>Phong.</i>

Sao rồi? #speaker:Nam #sprite:nam_surprise
<i>"Tôi kiếm được rồi."</i> #speaker:Phong 
<i>"Tôi kiếm được...."</i> #speaker:Phong 
<i>"Dark Soup 3 mới ra mắt, chơi luôn không anh bạn."</i> #speaker:Phong 
Nghe nói game lần này giải đố nhiều lắm đấy! #speaker:Phong 
...
Nam nhìn cuốn sách vừa cầm trên tay.
"Tôi về liền." #speaker:Nam #sprite:nam_excited
"Ai thua bao kèo đi ăn." #speaker:Nam #sprite:nam_angry
<i>"Oke tôi lại chấp cậu luôn cơ!"</i> #speaker:Phong
-> DONE

=== hallway2_monitor ===
{ ch1_hallway2_monitor_hasInteracted == false:
    ~ ch1_hallway2_monitor_hasInteracted = true
    Nam ngồi xuống trước chiếc máy tính công cộng.
    Màn hình phản chiếu khuôn mặt của Nam.
    ...
    Chỉ vài phút sau, những video đề xuất đầy màu sắc đã chen kín góc màn hình.
    ...
    ...
    Màn hình máy tính đột nhiên xuất hiện dòng chữ.
    "<color=Red>YOU DELAYED</color>".
    Nam khẽ chớp mắt.
    "Mình đang làm cái gì vậy..." #speaker:Nam #sprite:nam_angry
    Mình cần tập trung tìm kiếm. #speaker:Nam #sprite:nam_angry
    -> DONE
- else:
     "<color=Red>YOU DELAYED</color>".
    -> DONE
}
-> DONE

=== hallway2_paper ===
{ ch1_hallway2_paper_hasRead == false:
    ~ ch1_hallway2_paper_hasRead = true

    Một tờ giấy nhỏ nằm trên bàn.
    "<b>GitGud</b>".
    ...
    Nam nhìn tờ giấy một lúc.
    Rồi cậu đứng dậy. 
    Phải đi kiếm ý tưởng tiếp thôi. #speaker:Nam #sprite:nam_angry
    -> DONE
- else: 
    Nam lật mặt sau tờ giấy, có dòng chữ nhỏ dễ bị bỏ qua.
    "Coder <s>May</s> Cry".
    -> DONE
}

// ----------- Trong map1 hallway3 ----------
=== hallway3_librarian ===
{ ch1_hallway3_librarian == false:
    ~ ch1_hallway3_librarian = true
    Em kiếm được sách của em chưa? #speaker:Thủ thư
    Có rồi chị ơi. #speaker:Nam #sprite:nam_talk1
    Em thấy cuốn này cũng hay nên định mang về đọc thử. #speaker:Nam #sprite:nam_talk1

    Người thủ thư nhìn bìa sách. #speaker:Thủ thư
    ...
    Hướng dẫn làm game à, làm chị nhớ đến khoá trước cũng có người làm đồ án game. #speaker: Thủ thư 
    
    Thật vậy hả chị? #speaker:Nam #sprite:nam_surprise
    
    Ừ. #speaker:Thủ thư 
    Mấy bạn đó cũng từng quanh quẩn ở khu cuối thư viện để tìm tài liệu thêm. #speaker:Thủ thư 
     
    Nam nhìn về phía cuối thư viện.
    ... #speaker:Nam #sprite:nam_talk2
    Vậy từ từ để em xem thêm một vòng nha chị. #speaker:Nam #sprite:nam_talk1

    Vậy thì chị đánh dấu cuốn này lại cho em trước. #speaker:Thủ thư 
    Có mà lấy thêm sách nhớ quay lại chỗ chị nhá. #speaker:Thủ thư 
    À vâng. #speaker:Nam #sprite:nam_talk1
    -> DONE

- else:
    Nhớ trả sách vào tuần sau nhé. #speaker:Thủ thư 
    -> DONE
}

=== hallway3_door ===
{ch1_hallway3_door:
- 0:
    ~ ch1_hallway3_door = 1
    Em có mượn sách thư viện không em ơi ?? #speaker:Thủ thư 
    Nếu có thì lại đây để chị đóng dấu đã nhé!  #speaker:Thủ thư 
    -> DONE
- else: 
    ~ ch1_hallway3_door = 2
    Đợi chị đóng dấu sách đã em trai!! #speaker:Thủ thư 
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
=== puzzle3_atGoal ===
"Reng Reng!!!"
<i>"Về chưa đấy."</i> #speaker:Phong
<i>"Không về nhanh cẩn thận tôi chơi trước đấy nhá."</i> #speaker:Phong
->ch1_cutscene3


// ----------- Cutscene sau khi chơi game xong tại phòng trọ ----------
=== ch1_cutscene3 ===
#cg:timeskip_afternoon,5,full #sfx:clock_ticking
#bgm:vn_theme
#bg:dom_friend_afternoon

Màn hình tắt.
Tiêu đề trò chơi hiện ra.
<b><i>[GAME CLEAR]</i></b>

Cuốn thật. #speaker:Nam #sprite:nam_smile
Nhìn đơn giản mà căng phết. #speaker:Phong #sprite:phong_smile
Ừ. #speaker:Nam #sprite:nam_smile

Phong đặt tay cầm xuống bàn.
Tôi lại thích kiểu puzzle nhanh gọn hơn. #speaker:Phong #sprite:phong_thinking2
Mỗi màn một cái là xong. #speaker:Phong #sprite:phong_smile

Nam gật đầu nhẹ.
Giống tôi. #speaker:Nam #sprite:nam_agree

...

À mà. #speaker:Nam #sprite:nam_thinking
Phong nhìn sang.
Hôm nay tôi lên thư viện. #speaker:Nam #sprite:nam_talk1
Kiếm được gì không? #speaker:Phong #sprite:phong_talk1
Ban đầu toàn sách linh tinh. #speaker:Nam #sprite:nam_talk1

{ch1_hallway2_bookshelf_book2:
    Nhưng mà có 1 cuốn sách khiến tôi chú ý. #speaker:Nam #sprite:nam_talk1
    Nó nói <i>"Nếu muốn sáng tạo, hãy làm thứ mà mình thật sự thích."</i> #speaker:Nam #sprite:nam_talk1
    Ban đầu nó không liên quan tới đồ án nên tôi cũng không để tâm lắm. #speaker:Nam #sprite:nam_talk1
    Giờ nghĩ lại thì câu đấy nó lại đúng... #speaker:Nam #sprite:nam_talk1
    ... #speaker:Phong #sprite:phong_thinking2
}

Trong đống linh tinh đấy thì tui kiếm được cuốn hướng dẫn lập trình game. #speaker:Nam #sprite:nam_idea
Thấy thú vị thì tui mang về xem thử. #speaker:Nam #sprite:nam_talk2
À mà nhắc mới nhớ. #speaker:Nam #sprite:nam_surprise
Chị thủ thư kể khoá trước cũng có nhóm làm đồ án game. #speaker:Nam #sprite:nam_talk1
Nghe nói làm ổn phết. #speaker:Nam #sprite:nam_thinking
Thật không? #speaker:Phong #sprite:phong_surprise

Nam không trả lời ngay.
Cậu nhìn màn hình tiêu đề của trò chơi vừa hoàn thành.
Những căn phòng khóa kín.
Những câu đố.
Những đáp án được ghép lại từng chút một.

//chuyển sang bgm_warm_discovery
#bgm:vn_theme
Anh em mình chơi game cũng lâu rồi nhỉ. #speaker:Nam #sprite:nam_talk1
Ừ thì... #speaker:Phong #sprite:phong_talk2
Cày nát bao nhiêu game rồi còn gì. #speaker:Phong #sprite:phong_smile
Vậy nếu tụi mình thử làm một trò chơi thì sao? #speaker:Nam #sprite:nam_idea

Game? #speaker:Phong #sprite:phong_surprise

Ừ. #speaker:Nam #sprite:nam_agree
Puzzle. #speaker:Nam #sprite:nam_talk1
Theo kiểu tụi mình thích. #speaker:Nam #sprite:nam_talk1

Căn phòng im lặng vài giây.

... #speaker:Phong #sprite:phong_thinking1
... #speaker:Phong #sprite:phong_thinking2

Nghe được đấy. #speaker:Phong #sprite:phong_surprise
Biết đâu sau này có người chơi game của tụi mình. #speaker:Phong #sprite:phong_smile

Nam nhìn cuốn sách lập trình game đặt trên bàn.
Quyển sách được mang về chỉ vì tò mò.
Không hiểu sao.
Mọi thứ bỗng nhiên kết nối lại với nhau.

Thế...bắt đầu từ đâu đây? #speaker:Phong #sprite:phong_talk2

Gameplay trước. #speaker:Nam #sprite:nam_idea
Sau đó mới tới map với cốt truyện. #speaker:Nam #sprite:nam_smile

Cả Nam và Phong gật gù đồng ý.
-> DONE
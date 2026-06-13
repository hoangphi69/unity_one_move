// =============================================
// CHAPTER 2: NHỮNG TRỞ NGẠI BẤT NGỜ
// =============================================
VAR ch2_lobby_interacted_door = false
VAR ch2_lobby2_phone = false
VAR ch2_lobby2_door = 0   
VAR ch2_missed_report = false
VAR ch2_hallway1_talk_to_phong = 0

=== ch2_l2_phone ===
{ ch2_lobby2_phone == false:
    -> phone_notification
- else:
    -> phone_notification_none
}

= phone_notification
~ ch2_lobby2_phone = true
Hiện tại bạn có 1 cuộc gọi.
-> phone_call

= phone_call
Màn hình điện thoại sáng lên trong căn phòng còn tối.
<i>Phong — 7:03 SA</i>
<i>"Alo đội kiến trúc sư 3D một thành viên."</i> #speaker:Phong
<i>"Có việc gấp. Ra quán cà phê cậu giới thiệu đó, gặp tôi."</i> #speaker:Phong
Nam nhìn giờ trên màn hình.
Bảy giờ lẻ ba phút sáng.
"Chuyện gì mà giờ này?" #speaker:Nam #sprite:nam_exhaust
<i>"Model 3D của cậu có vấn đề, tiện bàn đồ án luôn."</i> #speaker:Phong
<i>"À, lát còn họp với GV đừng quên."</i> #speaker:Phong
...
Nam bật dậy.
"Ừ, ra liền." #speaker:Nam #sprite:nam_surprise
-> DONE

= phone_notification_none
<i>Hiện tại bạn có 0 thông báo.</i>
-> DONE


=== ch2_l2_door ===
{ ch2_lobby2_phone == false:
     ~ ch2_lobby2_door++
    { ch2_lobby2_door:
    - 1:
        <i>"Reng!!!!!"
        Mới sáng sớm mà ai gọi đấy. #speaker:Nam #sprite:nam_exhaust
        -> DONE
    - 2: 
        <i>"Reng Reng!!!!"
        Cái điện thoại đâu rồi nhờ. #speaker:Nam #sprite:nam_exhaust
        -> DONE
    - else:
        <i><b><uppercase>"Reng Reng Reng Reng Reng Reng <br>Reng Reng Reng Reng Reng Reng <br>Reng Reng Reng Reng Reng Reng !!!!!"
        -> DONE
    }
}


=== ch2_cutscene1 ===
#cg:chapter2,3,full #sfx:collect
#cg:black,2,full
#bgm:vn_theme
#bg:coffee_shop_morning

Quán Kohii vào buổi sáng sớm.
Ánh nắng lọc qua khung cửa kính, trải dài trên những hàng bàn gỗ.
Mùi cà phê vừa pha xong thoang thoảng đâu đó.

Phong đã ngồi sẵn ở góc gần ổ điện.
Laptop đã mở. Cốc đồ uống vẫn chưa đụng đến.
Trên màn hình là một đống thứ mà, từ khoảng cách này, Nam vẫn chưa đọc được rõ ràng.

Cậu có vẻ mê quán này nhờ. #speaker:Nam #sprite:nam_confused
Quán này xịn lắm bro. #speaker:Phong #sprite:phong_surprise
Từ lúc cậu giới thiệu đến giờ tôi ra đây suốt. #speaker:Phong #sprite:phong_smile
Tôi đặt sẵn chỗ rồi, ngay gần ổ điện luôn nhá. #speaker:Phong #sprite:phong_talk
Được, rất là tinh tế đấy nhá. #speaker:Nam #sprite:nam_talk1
Nam kéo ghế ngồi xuống.
Cái cốc của Phong vẫn còn nguyên chỗ cũ.

Ngoài này yên tĩnh thật. #speaker:Phong #sprite:phong_talk
Hm. Công nhận. #speaker:Nam #sprite:nam_talk1
Chả có gì ngoài... #speaker:Phong #sprite:phong_talk

Phong xoay màn hình laptop về phía Nam.

<i><b>20 DÒNG BUG BÁO LỖI cậu ơi!!!</b></i> #speaker:Phong #sprite:phong_exhaust

Nam nhìn màn hình.
Đỏ. Phần lớn đều đỏ.
Cậu xem thử cái demo tôi build hôm qua bị vấn đề gì nào. #speaker:Phong #sprite:phong_talk
...

Ờ... chạy cũng được mà. #speaker:Nam #sprite:nam_confused
Nam kéo cửa sổ game sang một bên, bắt đầu lướt qua.
Sao cái tường kia mất màu rồi... #speaker:Nam #sprite:nam_confused
Còn mấy cái vật này bay lung tung thế? #speaker:Nam #sprite:nam_panic

Ừ... vấn đề đấy. #speaker:Phong #sprite:phong_exhaust
Một vài model cậu làm đang bị lỗi, tôi cần cậu sửa lại. #speaker:Phong #sprite:phong_talk

Nam không trả lời ngay.
Cậu kéo cửa sổ code của Phong ra xem.
Một khoảng lặng thời gian trôi qua...

Có đúng lỗi từ model của tôi không đấy. #speaker:Nam #sprite:nam_talk1
Sao cậu viết rối rắm thế này, tôi hiểu chỗ nào được. #speaker:Nam #sprite:nam_angry
Ừ thì tôi viết demo trước. #speaker:Phong #sprite:phong_talk
Nhưng model của cậu vốn đã bị lỗi rồi. #speaker:Phong #sprite:phong_talk
Hmm... #speaker:Nam #sprite:nam_thinking
Model của cậu đang bị mất texture với vị trí bị bay lung tung đấy, cậu biết không? #speaker:Phong #sprite:phong_talk
Thế thì lạ đấy, rõ ràng tôi làm bài bản rồi mà. #speaker:Nam #sprite:nam_talk1
Với lại code của cậu đang bùi nhùi vậy thì dùng model của tôi kiểu gì? #speaker:Nam #sprite:nam_angry
Model của cậu vốn bị lỗi rồi. #speaker:Phong #sprite:phong_angry
Tôi chỉ đặt vào game là nó lỗi, không phải do code đâu. #speaker:Phong #sprite:phong_angry
Đống model đấy tôi thức mấy đêm liên tục mà vẫn bị lỗi được à. #speaker:Nam #sprite:nam_angry
Ừ thì code của tôi cũng thế, ai biết được nó lỗi chỗ nào đâu. #speaker:Phong #sprite:phong_angry
Hai người nhìn nhau.
Rồi cùng nhìn xuống màn hình.
Những dòng đỏ vẫn ở đó, vẫn kiên nhẫn chờ đợi.

Nếu vậy thì... để tôi check lại model thử. #speaker:Nam #sprite:nam_thinking
Cậu có viết logic đúng không đấy? #speaker:Nam #sprite:nam_angry
... #speaker:Phong #sprite:phong_thinking
Thế cậu có thiết kế đúng tỉ lệ không? #speaker:Phong #sprite:phong_angry
Khéo nguyên nhân từ bên cậu mà ra ấy. #speaker:Phong #sprite:phong_angry
...

+ [Tiếp tục ý kiến]
    #bg:coffee_shop_morning

    ~ ch2_missed_report = true
    Nam thở dài.
    Giờ cả hai cùng giải quyết luôn đi. #speaker:Nam #sprite:nam_talk1
    Cứ thế này thì đồ án năm sau mới báo cáo được cậu ơi. #speaker:Nam #sprite:nam_talk1
    Cậu sửa phần code, tôi sửa model. #speaker:Phong #sprite:phong_talk
    Giải quyết cả hai cùng lúc. #speaker:Nam #sprite:nam_talk1 
    Ừ. #speaker:Phong #sprite:phong_talk
    -> DONE

+ [...]
    #bg:coffee_shop_morning
    Phong bỗng ngừng lại.
    Tôi với cậu có quên gì không? #speaker:Phong #sprite:phong_thinking
    Nam nhìn Phong.
    Quên gì? #speaker:Nam #sprite:nam_confused
    ... #speaker:Phong #sprite:phong_thinking
    -> report_with_gv

= report_with_gv
Đến giờ họp rồi. #speaker:Phong #sprite:phong_talk

Nam nhìn đồng hồ.
...Ừ. #speaker:Nam #sprite:nam_panic
Nãy giờ bàn tôi quên luôn. #speaker:Nam #sprite:nam_panic
Còn đống này chưa sửa xong chứ. #speaker:Nam #sprite:nam_panic

Agggghhh..... #speaker:Phong #sprite:phong_exhaust
Agggghhh..... #speaker:Nam #sprite:nam_exhaust

#bg:meeting_online
Aggggghh... Tại sao các em làm không xong hả?? #speaker:GV
Có chừng này chức năng mà không làm được à?! #speaker:GV
Nếu không làm được thì đăng ký đồ án để làm gì cho vừa nặng vừa mệt?! #speaker:GV
Mấy đứa có biết mấy nhóm khác làm xong nhiệm vụ tuần này rồi không? #speaker:GV

Nhưng các nhóm khác làm web mà cô ơi. #speaker:Phong #sprite:phong_talk
Với lại em có bàn với cô các chức năng lớn cần... #speaker:Phong #sprite:phong_talk

Cô không cần biết. #speaker:GV
Đã chia việc thì làm cho xong đi chứ!! #speaker:GV
Mỗi tuần làm đủ chức năng như cô yêu cầu. #speaker:GV
Code nào thì cũng là code như nhau. #speaker:GV

Nam mở miệng.
Rồi khép lại.

.... #speaker:Nam #sprite:nam_silent

Cô thấy mấy đứa hơi bị lười rồi đấy!! #speaker:GV
Khổ vì mấy đứa quá rồi đấy! #speaker:GV

// Sau cuộc họp
#cg:dark,2,full #sfx:clock_ticking
#bgm:vn_theme
#bg:coffee_shop_morning

Quán vẫn thế.
Ánh nắng vẫn còn đó.
Mùi cà phê vẫn thoang thoảng.
Cái cốc của Phong vẫn chưa được đụng đến.
... #speaker:Nam #sprite:nam_angry
... #speaker:Phong #sprite:phong_angry
'Lười' à. #speaker:Nam #sprite:nam_angry
Tôi với cậu thức đến 2 giờ sáng mấy đêm liên tục. #speaker:Nam #sprite:nam_angry
Mà bị gọi là 'lười'. #speaker:Nam #sprite:nam_angry
Phong không trả lời ngay.
Cậu nhìn ra ngoài cửa kính.
Haizz... #speaker:Phong #sprite:phong_exhaust
Thôi đừng quan tâm nữa. #speaker:Phong #sprite:phong_talk
Bà ấy không sửa bug thay mình được đâu. #speaker:Phong #sprite:phong_talk
-> DONE

=== ch2_h1_phong ===
// Kohii coffee (Unity)
{ ch2_h1_phong :
-1: 
    Vào việc nào anh bạn.#speaker:Phong #sprite:phong_talk

    Phong mở một cửa sổ trên màn hình.
    Đây là bản Unity của quán Kohii.  #speaker:Phong #sprite:phong_talk
    Nhìn thì ra, nhưng mà... #speaker:Nam #sprite:nam_talk1
    Sao có mấy chỗ bị màu lạ thế này??! #speaker:Nam #sprite:nam_confused
    Lại có vài object bay lung tung nữa chứ!! #speaker:Nam #sprite:nam_confused

    Nam nhìn màn hình.
    Rồi nhìn ra cái quán cà phê thật bên ngoài màn hình.
    Rồi nhìn lại màn hình.

    Cậu dựng lại cái quán này trong game. #speaker:Nam #sprite:nam_surprised
    Ừ, quen thuộc thì dựng dễ hơn. #speaker:Phong #sprite:phong_smile

    Một cái ghế đang nằm trên mặt bàn kể cả khi cái ghế đang đặt đúng vị trí.
    Còn cái ghế dài đâm xuyên qua tường làm như cái tường không hề tồn tại.

    Sao nhiều model mất hết texture đấy? #speaker:Nam #sprite:nam_confused
    Giờ ưu tiên sửa lỗi toạ độ trước đã, object cứ lung tung thế này tôi không code tiếp được. #speaker:Phong #sprite:phong_talk
    Vậy để tôi xử lý. #speaker:Nam #sprite:nam_talk1
    Nhờ cậu đấy. #speaker:Phong #sprite:phong_talk
    -> DONE

- 2:
    Sao lại có mấy cái <nobr><uppercase>máy tính cổ lỗ sĩ</uppercase></nobr> trong mấy cái map thế này. #speaker:Nam #sprite:nam_confused
    Tôi nghĩ để đấy nó sẽ thú vị. #speaker:Phong #sprite:phong_excited
    Nó là cái <color="red">"Red Herring"</color> đấy, nghĩa của nó là <color="red">"mồi nhử"</color> thì phải. #speaker:Phong #sprite:phong_talk
    Tôi định đặt đấy để <b>"người chơi tương tác với nó"</b> dù nó sẽ không có gì xảy ra cả. #speaker:Phong #sprite:phong_talk
    Nhưng mà cứ map nào đặt cái máy tính đấy đều bị lỗi cả. #speaker:Phong #sprite:phong_angry
    Nên giờ cậu sửa mấy cái máy tính trong map giúp tôi nhá. #speaker:Phong #sprite:phong_talk
    Tôi lỡ đặt ở cả 3 map đấy. #speaker:Phong #sprite:phong_talk
    Có khi cậu sửa xong thì giải quyết được gì đấy. #speaker:Phong #sprite:phong_talk

    Nam không nói gì ngay.

    Cậu biết không, tôi tin ở cậu lắm đấy. #speaker:Phong #sprite:phong_smile
    Sửa hộ tôi nhá. #speaker:Phong #sprite:phong_smile
    -> DONE

- else:
    Sửa hộ tôi đống bùi nhùi đó nhá. #speaker:Phong #sprite:phong_smile
    -> DONE
}

=== ch2_h1_door ===
{ ch2_h1_door:
-1:
    Mình cần thảo luận với Phong, không có thời gian chạy lung tung được. #speaker:Nam #sprite:nam_talk1
    ->DONE
-2:
    Từ từ đã nào, lại đây thảo luận tí đã. #speaker:Phong #sprite:phong_talk
    ->DONE
-else:
    <b> Đồ án ở phía này. </b> #speaker:Phong #sprite:phong_talk
    Phải quay lại thôi. #speaker:Nam #sprite:nam_talk1
    ->DONE
}

=== ch2_h2_phong ===
// Kohii coffee (Unity)
{ ch2_h2_phong:
-1:
    Tôi sửa xong lỗi toạ độ rồi đấy. #speaker:Nam #sprite:nam_talk1
    Kiểm tra lại trong game thử. #speaker:Nam #sprite:nam_talk1
    Phong click thử vài chỗ.
    Hmm. Lỗi toạ độ giải quyết xong rồi đấy. #speaker:Phong #sprite:phong_talk

    Thế là xong rồi nhỉ, còn lại của m— #speaker:Nam #sprite:nam_talk1

    Chưa thư giãn được đâu anh bạn. #speaker:Phong #sprite:phong_talk

    Nam dừng lại.
    Phòng kế bên đang bị lỗi một vài object mất texture. #speaker:Phong #sprite:phong_talk
    Cứ đặt model ra map là màu y như rằng sẽ bị bốc hơi. #speaker:Phong #sprite:phong_talk
    Tôi cũng chả hiểu lỗi này lắm nên sửa lại hộ tôi nhá. #speaker:Phong #sprite:phong_talk
    ...Haizz. #speaker:Nam #sprite:nam_exhaust
    Để tôi xem thử. #speaker:Nam #sprite:nam_talk1
    OK bro. #speaker:Phong #sprite:phong_talk
    -> DONE
-else:
    Kiểm tra thử mấy vật bị lỗi phòng kế bên nhá. #speaker:Phong #sprite:phong_talk
    ->DONE
}

=== ch2_h2_door ===
{ ch2_h2_door:
-1:
    Vẫn còn vấn đề đấy nhé, chưa nghỉ ngơi được đâu!!. #speaker:Phong #sprite:phong_talk
    ->DONE
-else:
    Phải quay lại bàn với Phong thôi. #speaker:Nam #sprite:nam_talk1
    Đến khi nào mới giải quyết xong đống này đây. #speaker:Nam #sprite:nam_bored
    ->DONE
}

=== ch2_h3_phong ===
// Kohii coffee (Unity)
{ ch2_h3_phong:
-1: 
    Lỗi position sửa xong rồi. #speaker:Nam #sprite:nam_talk1
    Object đặt đúng chỗ hết rồi đấy. #speaker:Nam #sprite:nam_talk1
    Tốt. #speaker:Phong #sprite:phong_talk
    Tôi mới sửa lại code ở đoạn nhà kho đấy. #speaker:Phong #sprite:phong_talk
    Cơ mà nó vẫn còn glitch một tí. #speaker:Phong #sprite:phong_talk
    Cậu kiểm tra giúp tôi map nhà kho nhá. #speaker:Phong #sprite:phong_talk
    Map đấy thiên về kỹ thuật hơn cậu sửa được không. #speaker:Phong #sprite:phong_talk
    ... #speaker:Nam #sprite:nam_thinking
    Được chứ. #speaker:Nam #sprite:nam_excited
    -> DONE

- else:
    Đến bây giờ phần lớn vấn đề đang được xử lý rồi. #speaker:Phong #sprite:phong_talk
    Cố cho xong thôi. #speaker:Phong #sprite:phong_talk
    -> DONE
}

=== ch2_h3_door ===
{ ch2_h3_door:
-1:
    <b><i>Còn bao nhiêu cái bug nữa mới xong đây!!</i></b> #speaker:Phong #sprite:phong_exhaust
    Phải quay lại thảo luận với Phong thôi. #speaker:Nam #sprite:nam_exhaust
    ->DONE
-else:
    Quay lại với Phong thảo luận tiếp thôi. #speaker:Nam #sprite:nam_bored
    ->DONE
}

=== ch2_cutscene2 ===
#cg:laptop_screen,2,full #sfx:8_bit
#cg:black,1,full
#bgm:vn_theme
#bg:coffee_shop_evening
...
....
<i>[BUILD SUCCESSFUL]</i>
Nam! Được rồi! #speaker:Phong #sprite:phong_smile
Hả?! Chạy thật rồi à?! #speaker:Nam #sprite:nam_excited
Chạy rồi! Nhìn này! #speaker:Phong #sprite:phong_smile
Nhân vật di chuyển bình thường luôn! #speaker:Phong #sprite:phong_smile
Vũng nước thì sao? #speaker:Nam #sprite:nam_talk1
Còn trượt tới cuối bản đồ không? #speaker:Nam #sprite:nam_smile
Hết rồi! #speaker:Phong #sprite:phong_smile
Texture cũng lên đầy đủ luôn! #speaker:Phong #sprite:phong_smile
...
NÓ CHẠY ĐƯỢC RỒI! #speaker:Nam #sprite:nam_excited
HAHAHAHA! #speaker:Phong #sprite:phong_smile
TÔI VỚI ÔNG LÀM ĐƯỢC RỒI! #speaker:Nam & Phong
WOOOOOHOOOOO!!!! #speaker:Nam & Phong
LÊN NÀO BRO!!! #speaker:Nam #sprite:nam_excited
#cg:high_five,1,full #sfx:clap

Hôm nay phải ăn mừng mới được. #speaker:Phong #sprite:phong_excited
#bg:coffee_shop_evening
Có tiếng bước chân tiến lại gần.
Lần đầu chị thấy hai đứa cười lớn vậy luôn đó. #speaker:Mai
Bọn em build chạy được rồi chị ơi! #speaker:Phong #sprite:phong_smile
Thật hả? #speaker:Mai
Chúc mừng hai đứa nha. #speaker:Mai
Vậy lần này chị đãi bánh cho hai đứa đấy nhá. #speaker:Mai

Dạ, tụi em cảm ơn chị. #speaker:Nam #sprite:nam_smile
Hai đứa làm chị nhớ đến lần đầu mở quán cà phê đấy chứ. #speaker:Mai
CHị tưởng chuẩn bị xong hết rồi, ai ngờ tới ngày khai trương lại phát sinh đủ thứ chuyện. #speaker:Mai

Vậy rồi sao chị? #speaker:Phong #sprite:phong_talk
Thì giải quyết từng chuyện một thôi. #speaker:Mai 
Lúc đấy chị phải xoay đi xoay lại quá chời mà. #speaker:Mai
Phải vất vả lắm mới làm vượt qua được. #speaker:Mai
Ohhhh...#speaker:Phong #sprite:phong_surprise
Hay vậy chị. #speaker:Nam #sprite:nam_talk1
Hihi! #speaker:Mai
Hai đứa cố gắng nha. #speaker:Mai
Dạ. #speaker:Nam #sprite:nam_smile
Chị Mai quay trở về quầy pha chế.


Cũng trễ rồi, giờ này về thôi. #speaker:Phong #sprite:phong_talk
Oke. #speaker:Nam #sprite:nam_talk1

Phong lúc này đóng phần mềm giả lập.
Đóng phần mềm lập trình.
Đóng thư mục project.
Rồi dừng lại ở một tab vẫn đang mở.
...
...
<i>[TASK DO:.]. </i>
<i>[TASK DO:..]. </i>
<i>[TASK DO:...]. </i>
<i>[TASK DO:03...]. </i>
...

Cậu chưa tắt máy à? #speaker:Nam #sprite:nam_talk1

....À ..Ừ. #speaker:Phong #sprite:phong_talk

Nam nghiêng người nhìn sang.

Cái gì vậy? #speaker:Nam #sprite:nam_confused

Không có gì. #speaker:Phong #sprite:phong_talk

Mai tính tiếp. #speaker:Phong #sprite:phong_talk

...

Nam xoay nhẹ laptop lại về phía mình.

Màn hình hiển thị danh sách công việc.
...
□ Thiết kế map
□ Puzzle mới
□ UI
□ Save / Load
□ Audio
□ Cutscene
□ Testing
□ ...
□ ...

Nam lướt xuống.
Rồi lướt xuống thêm lần nữa.
...
Sao còn nhiều vậy? #speaker:Nam #sprite:nam_confused
Phong không trả lời.
Nam cũng im lặng.

Sự bất ngờ xen lẫn cảm xúc "tụt mood" mang lại sự yên tĩnh tràn ngập trong tâm trí Nam và Phong. 
Ngoài cửa kính, trời đã muộn.
...
Mai lại làm thôi. #speaker:Nam #sprite:nam_talk
Mai mấy giờ? #speaker:Phong #sprite:phong_talk
Bảy giờ. #speaker:Nam #sprite:nam_talk1
Lại Kohii? #speaker:Phong #sprite:phong_talk
Lại Kohii. #speaker:Nam #sprite:nam_talk1
...
Phong gập laptop lại.
Thu dọn đồ đạc cùng Nam.

Về thôi. #speaker:Phong #sprite:phong_smile
Ừ. #speaker:Nam #sprite:nam_smile
...
...
...
<b>[TASK DO: 03 \| TASK DONE: 02]</b>
-> DONE

// =============================================
// CHAPTER 2: NHỮNG TRỞ NGẠI BẤT NGỜ
// =============================================
VAR ch2_lobby_interacted_door = false
VAR ch2_lobby2_phone = false
VAR ch2_lobby2_door = 0   
VAR ch2_missed_report = false
VAR ch2_hallway1_talk_to_phong = 0

=== lobby2_phone ===
{ ch2_lobby2_phone == false:
    -> phone_notification
- else:
    -> phone_notification_none
}

= phone_notification
~ ch2_lobby2_phone = true
Hiện tại bạn có 1 tin nhắn.
+ [Đọc tin nhắn]
    -> phone_call
-> DONE

= phone_call
Màn hình điện thoại sáng lên trong căn phòng còn tối.
<i>Phong — 7:03 SA</i>
<i>"Alo đội kiến trúc sư 3D một thành viên."</i> #speaker:Phong
<i>"Có việc gấp. Ra quán cà phê ông giới thiệu đó, gặp tôi."</i> #speaker:Phong
Nam nhìn giờ trên màn hình.
Bảy giờ lẻ ba phút sáng.
"Chuyện gì mà giờ này?" #speaker:Nam #sprite:nam_tired
<i>"Model 3D của ông có vấn đề, tiện bàn đồ án luôn."</i> #speaker:Phong
<i>"À, lát còn họp với GV đừng quên."</i> #speaker:Phong
...
Nam ngồi dậy.
"Ừ, ra liền." #speaker:Nam #sprite:nam_talk
-> DONE

= phone_notification_none
<i>Hiện tại bạn có 0 thông báo.</i>
-> DONE


=== lobby2_door ===
{ ch2_lobby2_phone == false:
     ~ ch2_lobby2_door++
    { ch2_lobby2_door:
    - 1:
        <i>_"Ting!!!!!"_
        Mới sáng sớm mà ai gọi đấy. #speaker:Nam #sprite:nam_talk
        -> DONE
    - 2: 
        <i>_"Ting Ting!!!!!"_ 
        Cái điện thoại đâu rồi nhờ. #speaker:Nam #sprite:nam_talk
        -> DONE
    - else:
        <i><b><uppercase>_"Ting Ting Ting Ting Ting Ting <br>Ting Ting Ting Ting Ting Ting <br>Ting Ting Ting Ting Ting Ting !!!!!"_
        -> DONE
    }
}


=== ch2_cutscene1 ===
//Kohii coffee
#cg:chapter2,3,full #sfx:collect
#bgm:vn_theme
#bg:coffe_shop_morning

Quán Kohii vào buổi sáng sớm.
Ánh nắng lọc qua khung cửa kính, trải dài trên những hàng bàn gỗ.
Mùi cà phê vừa pha xong thoang thoảng đâu đó.

Phong đã ngồi sẵn ở góc gần ổ điện.
Laptop mở. Cốc đồ uống chưa đụng đến.
Trên màn hình là một đống thứ mà, từ khoảng cách này, Nam vẫn chưa đọc được rõ ràng.

Ông có vẻ mê quán này nhờ. #speaker:Nam #sprite:nam_talk
Quán này xịn lắm ông ơi. #speaker:Phong #sprite:phong_smile
Từ lúc ông giới thiệu đến giờ tôi ra đây suốt. #speaker:Phong #sprite:phong_smile
Tôi đặt sẵn chỗ rồi, ngay gần ổ điện luôn nhá. #speaker:Phong #sprite:phong_talk
Được, rất là tinh tế đấy nhá. #speaker:Nam #sprite:nam_talk

Nam kéo ghế ngồi xuống.
Cái cốc của Phong vẫn còn nguyên chỗ cũ.

Ngoài này yên tĩnh thật. #speaker:Phong #sprite:phong_talk
Hm. Công nhận. #speaker:Nam #sprite:nam_talk
Chả có gì ngoài... #speaker:Phong #sprite:phong_talk

Phong xoay màn hình laptop về phía Nam.

<i><b>20 DÒNG BUG BÁO LỖI ông ơi!!!</b></i> #speaker:Phong #sprite:phong_panic

Nam nhìn màn hình.
Đỏ. Phần lớn đều đỏ.
Ông xem thử cái demo tôi build hôm qua bị vấn đề gì nào. #speaker:Phong #sprite:phong_talk
...

Ờ... chạy cũng được mà. #speaker:Nam #sprite:nam_confused
Nam kéo cửa sổ game sang một bên, bắt đầu lướt qua.
Sao cái tường kia mất màu rồi... #speaker:Nam #sprite:nam_confused
Còn mấy cái vật này bay lung tung thế? #speaker:Nam #sprite:nam_panic

Ừ... vấn đề đấy. #speaker:Phong #sprite:phong_sad
Một vài model ông làm đang bị lỗi, tôi cần ông sửa lại. #speaker:Phong #sprite:phong_talk

Nam không trả lời ngay.
Cậu kéo cửa sổ code của Phong ra xem.
Một khoảng lặng thời gian trôi qua...

Có đúng lỗi từ model của tôi không đấy. #speaker:Nam #sprite:nam_talk
Sao ông viết rối rắm thế này, tôi hiểu chỗ nào được. #speaker:Nam #sprite:nam_angry
Ừ thì tôi viết demo trước. #speaker:Phong #sprite:phong_talk
Nhưng model của ông vốn đã bị lỗi rồi. #speaker:Phong #sprite:phong_talk
Hmm... #speaker:Nam #sprite:nam_thinking
Model của ông đang bị mất texture với vị trí bị bay lung tung đấy, ông biết không? #speaker:Phong #sprite:phong_talk
Thế thì lạ đấy, rõ ràng tôi làm bài bản rồi mà. #speaker:Nam #sprite:nam_talk
Với lại code của ông đang bùi nhùi vậy thì dùng model của tôi kiểu gì? #speaker:Nam #sprite:nam_angry
Model của ông vốn bị lỗi rồi. #speaker:Phong #sprite:phong_angry
Tôi chỉ đặt vào game là nó lỗi, không phải do code đâu. #speaker:Phong #sprite:phong_angry
Đống model đấy tôi thức mấy đêm liên tục mà vẫn bị lỗi được à. #speaker:Nam #sprite:nam_angry
Ừ thì code của tôi cũng thế, ai biết được nó lỗi chỗ nào đâu. #speaker:Phong #sprite:phong_angry
Hai người nhìn nhau.
Rồi cùng nhìn xuống màn hình.
Những dòng đỏ vẫn ở đó, vẫn kiên nhẫn chờ đợi.

Nếu vậy thì... để tôi check lại model thử. #speaker:Nam #sprite:nam_thinking
Ông có viết logic đúng không đấy? #speaker:Nam #sprite:nam_angry
... #speaker:Phong #sprite:phong_thinking
Thế ông có thiết kế đúng tỉ lệ không? #speaker:Phong #sprite:phong_angry
Khéo nguyên nhân từ bên ông mà ra ấy. #speaker:Phong #sprite:phong_angry
...

+ [Tiếp tục ý kiến]
    ~ ch2_missed_report = true
    Nam thở dài.
    Giờ cả hai cùng giải quyết luôn đi. #speaker:Nam #sprite:nam_talk
    Cứ thế này thì đồ án năm sau mới báo cáo được ông ơi. #speaker:Nam #sprite:nam_talk
    Ông sửa phần code, tôi sửa model. #speaker:Phong #sprite:phong_talk
    Giải quyết cả hai cùng lúc. #speaker:Phong #sprite:phong_talk
    Ừ. #speaker:Nam #sprite:nam_talk
    -> DONE

+ [...]
    Phong bỗng ngừng lại.
    Tôi với ông có quên gì không? #speaker:Phong #sprite:phong_thinking
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

Agggghhh..... #speaker:Phong #sprite:phong_panic
Agggghhh..... #speaker:Nam #sprite:nam_panic

// [Tiếng "Agggghhh" overlap với tiếng GV]
#bg: video_call
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
#bg:coffe_shop_morning

Quán vẫn thế.
Ánh nắng vẫn còn đó.
Mùi cà phê vẫn thoang thoảng.
Cái cốc của Phong vẫn chưa được đụng đến.
... #speaker:Nam #sprite:nam_angry
... #speaker:Phong #sprite:phong_angry
'Lười' à. #speaker:Nam #sprite:nam_angry
Tôi với ông thức đến 2 giờ sáng mấy đêm liên tục. #speaker:Nam #sprite:nam_angry
Mà bị gọi là 'lười'. #speaker:Nam #sprite:nam_angry
Phong không trả lời ngay.
Cậu nhìn ra ngoài cửa kính.
Haizz... #speaker:Phong #sprite:phong_sigh
Thôi đừng quan tâm nữa. #speaker:Phong #sprite:phong_sigh
Bà ấy không sửa bug thay mình được đâu. #speaker:Phong #sprite:phong_sigh
-> DONE

=== hallway1_phong ===
// Kohii coffee (Unity)
{ hallway1_phong :
-1: 
    Vào việc nào anh bạn.#speaker:Phong #sprite:phong_talk

    Phong mở một cửa sổ trên màn hình.
    Đây là bản Unity của quán Kohii.  #speaker:Phong #sprite:phong_talk
    Nhìn thì ra, nhưng mà... #speaker:Nam #sprite:nam_talk
    Sao có mấy chỗ bị màu lạ thế này??! #speaker:Nam #sprite:nam_confused
    Lại có vài object bay lung tung nữa chứ!! #speaker:Nam #sprite:nam_confused

    Nam nhìn màn hình.
    Rồi nhìn ra cái quán cà phê thật bên ngoài màn hình.
    Rồi nhìn lại màn hình.

    Ông dựng lại cái quán này trong game. #speaker:Nam #sprite:nam_surprised
    Ừ, quen thuộc thì dựng dễ hơn. #speaker:Phong #sprite:phong_smile

    Một cái ghế đang xoay chậm chạp cách sàn hai mét.

    Sao nhiều model mất hết texture đấy? #speaker:Nam #sprite:nam_confused
    Giờ ưu tiên sửa lỗi toạ độ trước đã, object cứ lung tung thế này tôi không code tiếp được. #speaker:Phong #sprite:phong_talk
    Vậy để tôi xử lý. #speaker:Nam #sprite:nam_talk
    Nhờ ông đấy. #speaker:Phong #sprite:phong_talk
    -> DONE

- 2:
    À này tôi quên nói. #speaker:Phong #sprite:phong_talk
    Tôi có thêm chức năng mới là cái vũng nước. #speaker:Phong #sprite:phong_talk
    Đáng lẽ bước vào thì trượt lên 2 ô, nhưng bằng cách nào đó nó đang lỗi — trượt hết cả đường về phía trước. #speaker:Phong #sprite:phong_talk
    Tôi cũng lỡ đặt vào mấy map giải đố rồi. #speaker:Phong #sprite:phong_talk
    Nhớ lưu ý nhá. #speaker:Phong #sprite:phong_talk

    Nam không nói gì ngay.

    Ông.... #speaker:Nam #sprite:nam_talk
    Trượt hết cả đường về phía trước nghĩa là sao? #speaker:Nam #sprite:nam_talk
    Nghĩa là... trượt hết về phía trước. #speaker:Phong #sprite:phong_talk
    ...Tới tường à? #speaker:Nam #sprite:nam_talk
    Tới tường. #speaker:Phong #sprite:phong_smile

    Ông biết không, tôi tin ở ông lắm đấy. #speaker:Phong #sprite:phong_smile
    Sửa hộ tôi nhá. #speaker:Phong #sprite:phong_smile
    -> DONE

- else:
    Sửa hộ tôi đống bùi nhùi đó nhá. #speaker:Phong #sprite:phong_smile
    -> DONE
}

// === hallway1_table1 ===
// { hallway1_table1:
// -1: 
//     Bàn nhìn có vẻ không đúng lắm. #speaker:Nam #sprite:nam_thinking
//     Nhìn nó đang thiếu cái gì đó. #speaker:Nam #sprite:nam_thinking
//     -> DONE
// -2:
//     Hình như bàn đang bị \#@^!*&^%#*&%*&%#*%*&%#. #speaker:Nam #sprite:nam_thinking
//     Hoa văn bàn này có vẻ khác với các bàn khác thì phải. #speaker:Nam #sprite:nam_thinking
//     -> DONE
// -else:
//     \#@^!*&^%#*&%*&%#*%*&%_@&^_!_*@_*&#_&^, phải sửa lại lỗi này thôi. #speaker:Nam #sprite:nam_thinking
//     -> DONE
// }

=== hallway1_table1 ===
{ hallway1_table1:
-1: 
    Hiện tại vật lý đang trong quá trình cập nhật.
    Nếu bạn thấy <voffset=15px><rotate="20">vật thể</voffset></rotate> đang lơ lửng.
    Có thể hiểu rằng vật lý hiện tại đang không tồn tại.
    Xin vui lòng chờ.
    -> DONE
-else:
    Đang đợi bản cập nhật <voffset=-15px>vật lý</voffset>.
    Xin vui lòng tiếp tục chờ.
    -> DONE
}

=== ch2_hallway1_door ===
{ ch2_hallway1_door:
-1:
    Mình cần thảo luận với Phong, không có thời gian chạy lung tung được. #speaker:Nam #sprite:nam_talk
    ->DONE
-2:
    Từ từ đã nào, lại đây thảo luận tí đã. #speaker:Phong #sprite:phong_talk
    ->DONE
-else:
    <b> Đồ án ở phía này. </b> #speaker:Phong #sprite:phong_talk
    Phải quay lại thôi. #speaker:Nam #sprite:nam_talk
    ->DONE
}

=== hallway2_phong ===
// Kohii coffee (Unity)
{ hallway2_phong:
-1:
    Tôi sửa xong lỗi toạ độ rồi đấy. #speaker:Nam #sprite:nam_talk
    Kiểm tra lại trong game thử. #speaker:Nam #sprite:nam_talk
    Phong click thử vài chỗ.
    Hmm. Lỗi toạ độ giải quyết xong rồi đấy. #speaker:Phong #sprite:phong_talk

    Thế là xong rồi nhỉ, còn lại của m— #speaker:Nam #sprite:nam_talk

    Chưa thư giãn được đâu anh bạn. #speaker:Phong #sprite:phong_talk

    Nam dừng lại.
    Phòng kế bên đang bị lỗi một vài object mất texture. #speaker:Phong #sprite:phong_talk
    Cứ đặt model ra map là màu y như rằng sẽ bị bốc hơi. #speaker:Phong #sprite:phong_talk
    Tôi cũng chả hiểu lỗi này lắm nên sửa lại hộ tôi nhá. #speaker:Phong #sprite:phong_talk
    ...Haizz. #speaker:Nam #sprite:nam_exhaust
    Để tôi xem thử. #speaker:Nam #sprite:nam_talk
    OK bro. #speaker:Phong #sprite:phong_talk
    -> DONE
-else:
    Kiểm tra thử mấy cái vật bị lỗi phòng kế bên nhá. #speaker:Phong #sprite:phong_talk
    ->DONE
}

=== hallway2_table1 ===
{ hallway2_table1:
-1: 
    Thật ngạc nhiên.
    Một cái bàn hết sức bình thường.
    Nam đứng nhìn nó một lúc.
    ...
    ...
    ...
    Chỉ là một cái bàn. #speaker:Nam #sprite:nam_thinking
    -> DONE
-else:
    Vẫn là một cái bàn bình thường.
    Không có gì để xem ở đây. #speaker:Nam #sprite:nam_thinking
    -> DONE
}

=== ch2_hallway2_door ===
{ ch2_hallway2_door:
-1:
    Vẫn còn vấn đề đấy nhé, chưa nghỉ ngơi được đâu!!. #speaker:Phong #sprite:phong_talk
    ->DONE
-else:
    Phải quay lại bàn với Phong thôi. #speaker:Nam #sprite:nam_talk
    Đến khi nào mới giải quyết xong đống này đây. #speaker:Nam #sprite:nam_bored
    ->DONE
}

=== hallway3_phong ===
// Kohii coffee (Unity)
{ hallway3_phong:
-1: 
    Lỗi position sửa xong rồi. #speaker:Nam #sprite:nam_talk
    Object đặt đúng chỗ hết rồi đấy. #speaker:Nam #sprite:nam_talk
    Tốt. #speaker:Phong #sprite:phong_talk
    Còn cái map cuối, liên quan đến kỹ thuật hơn nên để tôi xử lý luôn. #speaker:Phong #sprite:phong_talk
    Ông nghỉ tay một chút đi. #speaker:Phong #sprite:phong_smile
    Tôi mà nghỉ thì ông làm xong không? #speaker:Nam #sprite:nam_talk
    Có chứ. #speaker:Phong #sprite:phong_smile
    Không gì làm khó được tôi mà. #speaker:Phong #sprite:phong_smile

    Từ phía quầy, có tiếng ai đó vừa bước ra.

    Helo Phong nha! #speaker:Owner #sprite:owner_smile
    Hôm nay cũng ra đây làm việc à? #speaker:Owner #sprite:owner_smile
    Đúng rồi ạ, hôm nay tụi em ra đây ngồi làm đồ án. #speaker:Phong #sprite:phong_smile
    Ngày nào cũng thấy Phong ở đây nha. #speaker:Owner #sprite:owner_talk
    Vậy thì hôm nay chị đãi em một món nhé. #speaker:Owner #sprite:owner_smile
    Xịn vậy chị ơi. #speaker:Phong #sprite:phong_talk
    Khách ruột của chị mà. #speaker:Owner #sprite:owner_smile

    // { item_voucher == true:
        À chị ơi, hôm nay em mang theo voucher nha chị. #speaker:Phong #sprite:phong_smile

        Nam liếc sang.
        Cái voucher mà cậu nhặt được ở thư viện — cậu đã nhét vào tay Phong từ hôm đó vì không biết dùng làm gì.

        Voucher này là nhận bánh limited của quán được làm bởi chính chị này. #speaker:Owner #sprite:owner_smile
        Mấy đứa ăn xong nhận xét bánh chị làm đấy nhá. #speaker:Owner #sprite:owner_smile
        Okie chị. #speaker:Phong #sprite:phong_smile
    // }
    -> DONE

- else:
    Đến bây giờ phần lớn vấn đề đang được xử lý rồi. #speaker:Phong #sprite:phong_talk
    Cố cho xong thôi. #speaker:Phong #sprite:phong_talk
    -> DONE
}

=== ch2_hallway3_door ===
{ ch2_hallway3_door:
-1:
    <b><i>Còn bao nhiêu cái bug nữa mới xong đây!!</i></b> #speaker:Phong #sprite:phong_exhaust
    Phải quay lại thảo luận với Phong thôi. #speaker:Nam #sprite:nam_exhaust
    ->DONE
-else:
    Quay lại với Phong thảo luận tiếp thôi. #speaker:Nam #sprite:nam_bored
    ->DONE
}

=== hallway3_table1 ===
{ hallway3_table1:
-1:
    Trong Nam thoáng qua một suy nghĩ...
    Bàn ghế được sắp xếp rất gọn trái lại với vẻ kỳ lạ buổi sáng.
    ->DONE
-else:
    Quán vắng vẻ thật đấy. #speaker:Nam #sprite:nam_thinking
    ->DONE
}

=== hallway3_table2 ===
{ hallway3_table2:
-1:
    ...
    ...
    Sau một thời gian Nam để ý cái bàn.
    Nam nhận ra cái bàn được lau rất kỹ càng.
    Chắc được lâu bởi nhân viên cực kỳ chu đáo. #speaker:Nam #sprite:nam_thinking
    ->DONE
-else:
    Một cái bàn sạch sẽ hoàn toàn bình thường. 
    ->DONE
}


=== ch2_cutscene2 ===
#cg:laptop_screen,2,full #sfx:8_bit
#cg:black,1,full
#bgm:vn_theme
#bg:coffee_shop_evening
Nào, chứng kiến thời khắc huy hoàng nào anh bạn. #speaker:Nam #sprite:nam_excited
//Màn hình loading buid success
...
...
Nó đang chạy này. #speaker:Nam #sprite:nam_talk
Nó chạy được rồi này. #speaker:Nam #sprite:nam_smile
NÓ CHẠY ĐƯỢC RỒI NÀY. #speaker:Phong #sprite:phong_smile
TÔI VỚI ÔNG LÀM ĐƯỢC RỒI. #speaker:Nam & Phong
WOOOOOHOOOOO!!!! #speaker:Nam & Phong
LÊN NÀO BRO. #speaker:Nam #sprite:nam_excited
#cg:high_five,2,full #sfx:clap
#bg:coffee_shop_evening
Hai đứa cố gắng thật đấy chứ. #speaker:Owner #sprite:owner_talk
Trời tối thế này rồi hai đứa vẫn không nhận ra cơ mà. #speaker:Owner #sprite:owner_talk
Khách chị còn lại mỗi hai em thôi đấy. #speaker:Owner #sprite:owner_talk
Hehe, bọn em tập trung quá không nhận ra. #speaker:Phong #sprite:phong_smile
Bọn em làm phiền chị rồi. #speaker:Nam #sprite:nam_talk
Không sao, bình thường chỉ có Phong ở lại nói chuyện với chị. #speaker:Owner #sprite:owner_talk
Mai hai đứa tới ủng hộ chị tiếp nha. #speaker:Owner #sprite:owner_talk
Càng đông càng vui mà. #speaker:Owner #sprite:owner_smile

Chị chủ quán nhiệt tình quá ông ơi. #speaker:Nam #sprite:nam_smile
Ông hiểu tại sao tôi thích quán này rồi đấy, dù ông là ngưởi giới thiệu cho tôi. #speaker:Phong #sprite:phong_smile
-> DONE


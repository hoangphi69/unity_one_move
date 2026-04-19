// =============================================
// CHAPTER 2: NHỮNG TRỞ NGẠI BẤT NGỜ
// =============================================

// glitch -> bug

// Lobby
// Nhan cuoc goi cua Phong:
// - Lai nhai ve bug model cua Nam
// - Nhac hen hop voi gv
// - Chay ra quan cf

// Cutscene
// - Bong dua 1 ty
// - Phong phan nan Nam
// - Nam phan nan Phong
// - GV phan nan 2 dua
// - Nam & Phong quyet dinh sua lai 

// Hallway 1 (coffee)
// - Phong nho Nam sua map 1 (main)
// - Staff (nam)
// - Radio
// - Glitches

// Puzzle 1 (Nam) (lỗi texture)

// Hallway 2
// - Nam report voi Phong, Phong nho Nam sua bug tiep
// - Staff (nu)
// - Easter egg

// Puzzle 2 (Nam) (Lỗi position pivot)

// Hallway 3
// - Nam report voi Phong, sua loi con lai
// - Chủ quán (phải mặc đồ maid - !IMPORTANT)
//  + Unlock secret map neu co voucher
//  + normal interaction (phải thân tình - !IMPORTANT)
// - Easter egg
// - 


// Puzzle 3 (Phong) (Lỗi kỹ thuật)

// Cutscene
// - Phong build xong con game (demo) va chay dc
// - Phong & Nam han hoan an mung (cung voi chi chu quan mac do maid (important) ban phao dang sau background) (CG)
// - Nam cắm flag (đcm Nam)

#INCLUDE ch1.ink
VAR ch2_lobby_interacted_door = false
VAR ch2_lobby_door = 0
VAR ch2_missed_report = false
VAR ch2_hallway1_talk_to_phong = 0

=== ch2_lobby ===
{ ch2_lobby_interacted_door == false:
    -> phone_date
- else:
    { phone_call == 0:
        -> phone_notification
    - else:
        -> phone_notification_none
    }
}

= phone_date
<i>Ngày 15 tháng 3 - 7:05.
-> DONE

= phone_notification
Hiện tại bạn có 1 tin nhắn.
+ [Đọc tin nhắn]
    -> phone_call
+ [Bỏ qua tin nhắn]
    -> phone_notification_none
-> DONE

= phone_call
Alo đội kiến trúc sư 3D một thành viên. #speaker:Phong #sprite:phong_talk
Có việc gấp đây, cần mày xử lý. #speaker:Phong #sprite:phong_talk
Nhớ cái quán cà phê mày giới thiệu không, ra đó gặp tao. #speaker:Phong #sprite:phong_talk
Ơ... chuyện gì mà gấp thế? #speaker:Nam #sprite:nam_talk
Model 3D của mày đang có vấn đề, tiện bàn luôn đồ án. #speaker:Phong #sprite:phong_talk
À mà lát còn họp với GV, đừng quên. #speaker:Phong #sprite:phong_talk
Ừ, ra liền. #speaker:Nam #sprite:nam_talk
-> DONE

= phone_notification_none
Hiện tại bạn có 0 thông báo.
-> DONE

=== lobby_door ===
{ ch2_lobby_door:
-1:
    ~ ch2_lobby_interacted_door = true
    <i>_"Ting!!!!!"_
    Mới sáng sớm mà ai gọi đấy. #speaker:Nam #sprite:nam_talk
    -> DONE
-2: 
    <i>_"Ting Ting!!!!!"_ 
    Cái điện thoại đâu rồi nhờ. #speaker:Nam #sprite:nam_talk
    -> DONE
-else:
    <i><b><uppercase>_"Ting Ting Ting Ting Ting Ting <br>Ting Ting Ting Ting Ting Ting <br>Ting Ting Ting Ting Ting Ting !!!!!"_
    -> DONE
}


=== ch2_cutscene1 ===
//Kohii coffee
#bg:coffe_shop_morning
Mày có vẻ mê quán này nhờ. #speaker:Nam #sprite:nam_talk
Quán này xịn lắm mày ơi. #speaker:Phong #sprite:phong_smile
Từ lúc mày giới thiệu tao đến giờ tao ra đây suốt. #speaker:Phong #sprite:phong_smile
Tao đặt sẵn chỗ rồi, ngay gần ổ điện luôn nhá. #speaker:Phong #sprite:phong_talk
Được, rất là tinh tế đấy nhá. #speaker:Nam #sprite:nam_talk

Ngoài này yên tĩnh thật ấy chứ. #speaker:Phong #sprite:phong_talk
Hmmm... công nhận. #speaker:Nam #sprite:nam_talk
Chả có gì ngoài... #speaker:Phong #sprite:phong_talk
<i><b>20 DÒNG BUG BÁO LỖI mày ơi!!! #speaker:Phong #sprite:phong_panic

Mày xem thử cái demo tao build hôm qua bị vấn đề gì nào. #speaker:Phong #sprite:phong_talk
... #speaker:Nam #sprite:nam_thinking
Ờ... chạy cũng được mà, sao bị lỗi được. #speaker:Nam #sprite:nam_panic
Từ từ nào, sao cái tường kia mất màu rồi, thêm mấy cái vật này bay lung tung thế? #speaker:Nam #sprite:nam_panic

Ừ... vấn đề đấy, hiện tại một vài model mày làm đang bị lỗi, tao đang cần mày sửa lại cho tao. #speaker:Phong #sprite:phong_talk
Có đúng lỗi từ model của tao không đấy, đưa tao xem code của mày nào. #speaker:Nam #sprite:nam_talk
Sao mày viết rối rắm thế, mày viết kiểu này sao tao hiểu được. #speaker:Nam #sprite:nam_angry
Ừ thì tao viết demo trước, nhưng model của mày vốn đã bị lỗi rồi. #speaker:Phong #sprite:phong_talk

Hmm... #speaker:Nam #sprite:nam_thinking
Model của mày đang bị lỗi mất texture với vị trí bị bay lung tung đấy, biết không? #speaker:Phong #sprite:phong_talk
Hmm... thế thì lạ đấy, rõ ràng tao làm bài bản rồi mà. #speaker:Nam #sprite:nam_talk
Với lại code của mày đang bùi nhùi vậy thì dùng model của tao kiểu gì? #speaker:Nam #sprite:nam_angry

Model của mày vốn bị lỗi rồi, tao chỉ đặt vào game là nó lỗi, không phải do code đâu. #speaker:Phong #sprite:phong_angry
Đống model đấy tao thức mấy đêm làm liên tục mà vẫn bị lỗi được à. #speaker:Nam #sprite:nam_angry
Ừ thì code của tao cũng thế, ai biết được nó lỗi chỗ nào đâu. #speaker:Phong #sprite:phong_angry

Hmmm... để tao check lại model thử, mày có viết logic đúng không đấy!? #speaker:Nam #sprite:nam_angry
... #speaker:Phong #sprite:phong_thinking
Thế mày có thiết kế đúng tỉ lệ không?! #speaker:Phong #sprite:phong_angry
Khéo nguyên nhân từ bên mày mà ra ấy!! #speaker:Phong #sprite:phong_angry
... #speaker:Nam #sprite:nam_thinking

+ [Đề xuất ý tưởng]
    ~ ch2_missed_report = true
    Giờ cả 2 cùng tìm cách giải quyết vấn đề luôn. #speaker:Nam #sprite:nam_talk
    Cứ thế này thì đồ án năm sau mới báo cáo được mày ơi. #speaker:Nam #sprite:nam_talk
    Vậy tao sửa phần code, mày sửa phần model đi, giải quyết cả hai vấn đề cùng lúc luôn. #speaker:Phong #sprite:phong_talk
    Ừ. #speaker:Nam #sprite:nam_talk
    -> DONE

+ [...]
    Từ từ nào. #speaker:Phong #sprite:phong_talk
    Tao với mày có quên gì không? #speaker:Phong #sprite:phong_talk
    Quên gì? #speaker:Nam #sprite:nam_talk
    ... #speaker:Phong #sprite:phong_thinking
    -> report_with_gv

= report_with_gv
Đến giờ họp rồi. #speaker:Phong #sprite:phong_talk
... Ừ, nãy giờ bàn tao quên luôn. #speaker:Nam #sprite:nam_panic
Còn đống này chưa sửa xong chứ. #speaker:Nam #sprite:nam_panic
Agggghhh..... #speaker:Phong #sprite:phong_panic
Agggghhh..... #speaker:Nam #sprite:nam_panic

// Trong lúc họp 
#bg:video_call
Aggggghh... Tại sao các em làm không xong hả?? #speaker:GV
Có chừng này chức năng các em có làm được không? #speaker:GV
Nếu không làm được thì các em đăng ký đồ án tốt nghiệp để làm gì cho vừa nặng vừa mệt? #speaker:GV
Mấy đứa có biết mấy nhóm khác làm xong nhiệm vụ tuần này rồi không? #speaker:GV
Nhưng các nhóm khác làm web mà cô ơi, với lại em có bàn với cô các chức năng lớn cần...... #speaker:Phong #sprite:phong_talk
Cô không cần biết. #speaker:GV
chia việc ra rồi thì làm cho xong đi chứ!! #speaker:GV
Mỗi tuần làm đủ chức năng như cô yêu cầu. #speaker:GV
Code nào thì cũng là code như nhau. #speaker:GV
.... #speaker:Nam #sprite:nam_talk
.... #speaker:Nam #sprite:nam_silent
Cô thấy mấy đứa hơi bị lười rồi đấy!! #speaker:GV
Khổ vì mấy đứa quá rồi đấy! #speaker:GV

// Sau cuộc họp
#bg:coffe_shop_morning
... #speaker:Nam #sprite:nam_angry
... #speaker:Phong #sprite:phong_angry
"lười" à. #speaker:Nam #sprite:nam_angry
Tao với mày thức đến 2 giờ sáng mấy đêm liên tục. #speaker:Nam #sprite:nam_angry
MÀ BỊ GỌI LÀ "LƯỜI".  #speaker:Nam #sprite:nam_angry
Haizz... #speaker:Phong #sprite:phong_sigh
Thôi đừng quan tâm đến nữa. #speaker:Phong #sprite:phong_sigh
Bà ấy không sửa bug thay mình được đâu. #speaker:Phong #sprite:phong_sigh
-> DONE


=== ch2_hallway1 ===
// Kohii coffee (Unity)
{ ch2_hallway1 :
-0: 
    Vào việc nào anh bạn. #speaker:Phong #sprite:phong_talk
    Như mày thấy rồi đấy! #speaker:Phong #sprite:phong_talk
    Tao dựng map này lấy bối cảnh quán cà phê. #speaker:Phong #sprite:phong_talk
    Nhưng nó đang bị lỗi gì đó tao chưa rõ. #speaker:Phong #sprite:phong_talk
    Tao giao lại cho mày nhé? #speaker:Phong #sprite:phong_talk
    Mày... dựng lại cái quán này trong game? #speaker:Nam #sprite:nam_surprise
    Ừ, tao thấy quen thuộc thì dễ dựng map hơn. #speaker:Phong #sprite:phong_smile

    Giờ mày mà đi tới cái map tầng trên đấy, trên đấy có lỗi mà tao không biết. #speaker:Phong #sprite:phong_talk
    Sao nhiều model mất hết texture đấy? #speaker:Nam #sprite:nam_confused
    À thôi tao biết lỗi này rồi. #speaker:Nam #sprite:nam_talk
    Cái này để tao sửa. #speaker:Nam #sprite:nam_talk
    Thế thì nhờ mày đấy anh bạn. #speaker:Phong #sprite:phong_talk
    -> DONE

-1: 
    À này tao quên nói. #speaker:Phong #sprite:phong_talk
    Tao có thêm chức năng mới là có cái vũng nước đấy, đáng lẽ nó sẽ trượt lên 2 ô nhưng bằng cách nào đấy nó đang bị lỗi trượt hết cả đường đi về phía trước. #speaker:Phong #sprite:phong_talk
    Tao cũng có lỡ đặt ở trong mấy cái map giải đố. #speaker:Phong #sprite:phong_talk
    Nhớ lưu ý nhá. #speaker:Phong #sprite:phong_talk
    Mà với mày thì chắc tìm được hướng đi mới nhờ vào nó mà nhỉ. #speaker:Phong #sprite:phong_smile
    Tao tin ở mày.  #speaker:Phong #sprite:phong_smile
    -> DONE

-else:
    Sửa hộ tao đóng bùi nhùi đấy nhá. #speaker:Phong #sprite:phong_smile
    -> DONE
}

=== ch2_hallway1_table1 ===
{ ch2_hallway1_table1:
-0: 
    Bàn nhìn có vẻ không đúng lắm. #speaker:Nam #sprite:nam_thinking
    Nhìn nó đang thiếu cái gì đó. #speaker:Nam #sprite:nam_thinking
    -> DONE
-1:
    Hình như bàn đang bị \#@^!*&^%#*&%*&%#*%*&%#. #speaker:Nam #sprite:nam_thinking
    -> DONE
-else:
    \#@^!*&^%#*&%*&%#*%*&%_@&^_!_*@_*&#_&^, phải sửa lại lỗi này thôi. #speaker:Nam #sprite:nam_thinking
    -> DONE
}

=== ch2_hallway1_table2 ===
{ ch2_hallway1_table2:
-0: 
    Cái <voffset=15px><rotate="-10">bàn</voffset></rotate> không ở đúng vị trí của nó. #speaker:Nam #sprite:nam_thinking
    -> DONE
-else:
    Bàn đang <voffset=15px><rotate="-10">cao</voffset></rotate> hơn bình thường thì phải. #speaker:Nam #sprite:nam_thinking
    -> DONE
}


=== ch2_hallway2 ===
// Kohii coffee (Unity)
{ ch2_hallway2:
-0:
    Tao sửa xong lỗi texture rồi đấy. #speaker:Nam #sprite:nam_talk 
    Kiểm tra lại trong game thử. #speaker:Nam #sprite:nam_talk 
    Hmm....Ừm, lỗi texture mày giải quyết xong rồi đấy. #speaker:Phong #sprite:phong_talk
    Thế là giải quyết xong rồi nhể, còn lại của m.... #speaker:Nam #sprite:nam_talk
    Chưa thư giãn được đâu anh bạn. #speaker:Phong #sprite:phong_talk
    Phòng kế bên đang bị lỗi mấy cái object đang bay lung tung đấy. #speaker:Phong #sprite:phong_talk
    Tao cứ đặt model ra là nó cứ bay lung tung. #speaker:Phong #sprite:phong_talk
    Sửa lại hộ tao nhá. #speaker:Phong #sprite:phong_talk
    Haizz.. #speaker:Nam #sprite:nam_exhaust
    Rồi để tao xem thử như nào. #speaker:Nam #sprite:nam_talk
    Ừm, cố lên nhá. #speaker:Phong #sprite:phong_talk
    -> DONE
-1:
    
-else:
    ->DONE
}

=== ch2_hallway2_table1 ===
{ ch2_hallway2_table1:
-0: 
    Thật ngạc nhiên. #speaker:Nam #sprite:nam_thinking
    Một cái bàn hết sức bình thường. #speaker:Nam #sprite:nam_thinking
    -> DONE
-else:
    Chỉ là một cái bàn bình thường. #speaker:Nam #sprite:nam_thinking
    -> DONE
}

=== ch2_hallway2_table2 ===
{ ch2_hallway2_table2:
-0: 
    Hiện tại vật lý đang trong quá trình cập nhật.
    Nếu bạn thấy <voffset=15px><rotate="20">cái bàn</voffset></rotate> đang lơ lửng.
    Có thể hiểu rằng vật lý hiện tại đang không tồn tại.
    -> DONE
-else:
    Đang đợi <voffset=-15px>vật lý</voffset> cập nhật.
    Xin vui lòng cập nhật.
    -> DONE
}

=== ch2_hallway3 ===
// Kohii coffee (Unity)
Lỗi position tao sửa xong rồi. #speaker:Nam #sprite:nam_talk
Object đặt đúng chỗ hết rồi đấy. #speaker:Nam #sprite:nam_talk

Tốt. #speaker:Phong #sprite:phong_talk
Giờ còn cái map cuối, cái này liên quan đến kỹ thuật hơn nên để tao xử lý luôn. #speaker:Phong #sprite:phong_talk
Mày nghỉ tay một chút đi. #speaker:Phong #sprite:phong_smile

Tao mà nghỉ thì mày làm xong không? #speaker:Nam #sprite:nam_talk
Có chứ. #speaker:Phong #sprite:phong_smile
Tao giỏi mà. #speaker:Phong #sprite:phong_smile

Ủa, Phong! Hôm nay cũng ra đây làm việc à? #speaker:Owner #sprite:owner_smile
Đúng rồi ạ, hôm nay tụi em ra đây ngồi làm đồ án. #speaker:Phong #sprite:phong_smile
Ngày nào cũng thấy Phong ở đây nha. #speaker:Owner #sprite:owner_talk
Để chị lấy phần của em ra nhé, chị nhớ order của em rồi. #speaker:Owner #sprite:owner_smile
Chị nhớ order của em luôn à. #speaker:Phong #sprite:phong_suprise
Khách ruột của chị mà. #speaker:Owner #sprite:owner_smile

// { item_voucher == true:
    À chị ơi. #speaker:Phong #sprite:phong_smile
    Bữa nay em mang theo voucher nha chị. #speaker:Phong #sprite:phong_smile
    Voucher mình nhặt được ở thư viện à. #speaker:Nam #sprite:nam_thinking
    Mình cho Phong do mình cũng không hứng thú lắm. #speaker:Nam #sprite:nam_thinking
    Vậy em muốn món gì nè. #speaker:Owner #sprite:owner_talk
    Miễn phí 1 món trên menu nha. #speaker:Owner #sprite:owner_smile
    Vậy em lấy bánh đặc biệt nhất nha. #speaker:Phong #sprite:phong_smile
    Okie nha Phong. #speaker:Owner #sprite:owner_smile
    ...
    ...
    Phong ơi!!! #speaker:Owner #sprite:owner_talk 
    Hiện tại chị đang hết nguyên liệu, chị còn phải trông quán. #speaker:Owner #sprite:owner_talk 
    Em lấy nguyên liệu giúp chị nha, ở trong nhà kho bên cạnh đấy. #speaker:Owner #sprite:owner_talk 
    Oke chị. #speaker:Phong #sprite:phong_smile
// }

// Chị chủ quán nhìn sang Nam
Bạn của Phong à? #speaker:Owner #sprite:owner_talk
Bạn làm đồ án cùng em đấy ạ. #speaker:Phong #sprite:phong_smile
Ủa, Nam đó hả, lâu rồi mới thấy lại em nha. #speaker:Owner #sprite:owner_talk
... #speaker:Nam #sprite:nam_smile

Vậy chị lấy đồ uống cho em luôn nha. #speaker:Owner #sprite:owner_talk
Em cảm ơn chị. #speaker:Nam #sprite:nam_smile
Mấy đứa đang làm đồ án đúng không. #speaker:Owner #sprite:owner_talk
Cố gắng lên nha. #speaker:Owner #sprite:owner_smile
-> DONE

=== ch2_hallway3_table1 ===
{ ch2_hallway3_table1:
-0: 
    -> DONE
-else:
    -> DONE
}

=== ch2_hallway3_table2 ===
{ ch2_hallway3_table2:
-0: 
    -> DONE
-else:
    -> DONE
}

=== ch2_cutscene2 ===
Nào, chứng kiến thời khắc huy hoàng nào anh bạn. #speaker:Nam #sprite:nam_excited
//Màn hình loading buid success
...
...
Nó đang chạy này. #speaker:Nam #sprite:nam_talk
Nó chạy được rồi này. #speaker:Nam #sprite:nam_smile
NÓ CHẠY ĐƯỢC RỒI NÀY. #speaker:Phong #sprite:phong_smile
TAO VỚI MÀY LÀM ĐƯỢC RỒI. #speaker:Nam & Phong
WOOOOOHOOOOO!!!! #speaker:Nam & Phong
LÊN NÀO NGƯỜI ANH EM. #speaker:Nam #sprite:nam_excited
// Hai đứa đập tay nhau

#sprite:ChuQuanBanPhaoBong
Chúc mừng hai em nha!!! #speaker:Owner #sprite:owner_smile
BOOOMMMM!
Uầyyyyy!!! #speaker:Nam & Phong
Xịn vậy chị ơi. #speaker:Nam #sprite:nam_talk
Lâu lâu mới có dịp mà. #speaker:Owner #sprite:owner_talk
Với lại vì khách ruột của chị chứ. #speaker:Owner #sprite:owner_smile
Hai em làm xong rồi hả? #speaker:Owner #sprite:owner_talk
Chúc mừng hai em nha. #speaker:Owner #sprite:owner_smile
#sprite:nam&phong_smile


// Đập tay với chị chủ quán 
Hai đứa cố gắng thật đấy chứ. #speaker:Owner #sprite:owner_talk
Trời tối thế này rồi hai đứa vẫn không nhận ra cơ mà. #speaker:Owner #sprite:owner_talk
Khách chị còn lại mỗi hai em thôi đấy. #speaker:Owner #sprite:owner_talk
Hehe, bọn em tập trung quá không nhận ra. #speaker:Phong #sprite:phong_smile
Bọn em làm phiền chị rồi. #speaker:Nam #sprite:nam_talk
Không sao, bình thường chỉ có Phong ở lại nói chuyện với chị. #speaker:Owner #sprite:owner_talk
Mai hai đứa tới ủng hộ chị tiếp nha. #speaker:Owner #sprite:owner_talk
Càng đông càng vui mà. #speaker:Owner #sprite:owner_smile


Chị chủ quán nhiệt tình quá mày ơi. #speaker:Nam #sprite:nam_smile
Mày hiểu tại sao tao thích quán này rồi đấy, dù mày là ngưởi giới thiệu cho tao. #speaker:Phong #sprite:phong_smile
-> DONE
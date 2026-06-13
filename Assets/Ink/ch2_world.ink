// chapter 2 - lobby
// ---------

=== ch2_l1_bed ===
{ stopping:
  - Chiếc giường trông thật bừa bộn.
    Tuy nhiên đó là lẽ thường tình với bất cứ ai sống một mình, kể cả Nam.
    Mình sẽ dọn giường sau vậy. #speaker:Nam #sprite:nam_exhaust
  - Dọn giường để sau vậy. #speaker:Nam #sprite:nam_exhaust
}
-> DONE


=== ch2_l1_papers ===
{ once:
  - Trên bàn ngổn ngang nhiều loại giấy tờ, một số còn rải rác lộn xộn dưới sàn.
    Toàn bộ là tài liệu Nam tổng hợp được trong mấy tuần qua phục vụ cho đồ án mà cả hai đang làm.
}
Nhìn bừa bộn quá, mình phải sắp xếp lại sau thôi. #speaker:Nam #nam_silent
-> DONE


=== ch2_l1_window ===
{ stopping:
  - Chiếu qua cửa sổ là ánh nắng của buổi sáng mang theo sức sống của cảnh vật.
    Tiếng chim hót líu lo cùng tiếng gió lao xao qua kẽ lá tạo nên khúc âm hưởng của một buổi sáng sớm.
    ...Đâu đó có tiếng cãi cọ của hàng xóm vang vọng khắp khu nhà trọ.
    Mới sáng sớm mà họ đã om sòm thế nhỉ. #speaker:Nam #nam_exhaust
  - Chắc mai mốt mình phải lắp cách âm vào mới được. #speaker:Nam #nam_silent
}
-> DONE


// chapter 2 - hallway 1
// ---------

=== ch2_h1_clock ===
Đồng hồ điểm 9 giờ sáng.
-> DONE

=== ch2_h1_table1 ===
{ ch2_h1_table1:
-1: 
    Hiện tại vật lý đang trong quá trình cập nhật.
    Nếu bạn thấy <voffset=15px><rotate="20">vật thể</voffset></rotate> đang không đúng vị trí.
    Có thể hiểu rằng vật lý hiện tại đang không tồn tại.
    Xin vui lòng chờ.
    -> DONE
-else:
    Đang đợi bản cập nhật <voffset=-15px>vật lý</voffset>.
    Xin vui lòng tiếp tục chờ.
    -> DONE
}

=== ch2_h1_npc_maid ===
{!Nhân viên hiện tại là một cô gái trạc tuổi Nam, trong bộ trang phục hầu gái "moe" thường thấy ở Nhật.}
{ ch2_h1_npc_maid > 2: Cậu muốn gì?| Quý khách muốn gì ạ?} #speaker:Hầu gái
  + [Nói về vết đổ nước trên sàn]
    { stopping:
      - Ừm... Mình lỡ làm đổ nước lên sàn, cậu có thể cho tôi mượn khăn lau... hay đại loại gì đó được không? #speaker:Nam #nam_thinking
        Nàng hầu gái trầm ngâm một lúc.
        Cậu cứ để đó đi, chốc nữa tôi sẽ dọn cho. #speaker:Hầu gái
        Vậy sao, c-cảm ơn cậu nhiều. #speaker:Nam
        -> DONE
      - -> laidback
    }
  + [Nói về bàn ghế bị lỗi toạ độ]
    { stopping:
      - Ừm... Hình như bàn ghế ở góc có gì đó không đúng thì phải. #speaker:Nam
        Nàng hầu gái trầm ngâm một lúc.
        À, khu đó bọn nhóc vừa mới ra về. #speaker:Hầu gái
        Cứ để đó đi, tôi sẽ xử lý sau. #speaker:Hầu gái
        -> DONE
      - -> laidback
    }
  + [Bỏ đi]
    -> DONE

= laidback 
{shuffle:
  - Để đó tý tôi dọn. #speaker:Hầu gái
  - Tý tôi xử lý cho. #speaker:Hầu gái
  - Cứ để đó cho tôi. #speaker:Hầu gái
}
-> DONE


=== ch2_h1_npc_girlfriend ===
{ stopping:
  - Một cặp đôi có vẻ đang trong một cuộc trò chuyện "một chiều" rất rôm rả.
    Mặc cho đối phương vẫn thao thao bất tuyệt, cô gái tiếp tục hững hờ lướt điện thoại.
  - \*Bấm điện thoại\*
}
-> DONE


=== ch2_h1_npc_boyfriend ===
{ stopping: 
  - Một cặp đôi có vẻ đang trong một cuộc trò chuyện "một chiều" rất rôm rả.
    Có điều anh ta là thính giả duy nhất lắng nghe câu chuyện mình đang kể.
  - \*Nói không ngừng nghỉ\*
}
-> DONE


=== ch2_h1_npc_ceo ===
Chiếc bàn này có hoa văn thật thú vị. #speaker:Tổng tài
Phải mua chiếc bàn này ngay mới được. #speaker:Tổng tài
-> DONE


// chapter 2 - hallway 2
// ---------

=== ch2_h2_clock ===
Đồng hồ điểm 1 giờ trưa.
-> DONE

=== ch2_h2_table1 ===
{ ch2_h2_table1:
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

=== ch2_h2_npc_ceo ===
Chiếc bàn này có hoa văn thật thú vị. #speaker:Tổng tài
Có tiền là tôi mua luôn. #speaker:Tổng tài
-> DONE

=== ch2_h2_npc_butler ===
{ once:
  - Nhân viên ca hiện tại là một người đàn ông trong trang phục bồi bàn lịch lãm kiểu Tây Âu.
    Vì lý do gì đó, Nam cảm thấy không thoải mái với bộ ria mép của ông ta.
}
- (ask) Tôi có thể giúp được gì cho cậu, chàng trai trẻ? #speaker:"Quản gia"
  + [Hỏi về hầu gái ca trước]
    { stopping: 
      - Cô gái ca trước hả? Chà... mặc cho vẻ ngoài sống chậm của cô ấy, #speaker:"Quản gia"
        Có thể nói cô ấy là nhân viên chăm chỉ nhất quán đấy. #speaker:"Quản gia"
        Thật ư? #speaker:Nam
        Chắc chắn mà. #speaker:"Quản gia"
      - Mọi người còn gọi cô ấy là... "cô gái thư giãn". #speaker:"Quản gia"
    }
    -> ask
  + (doubt) [Hỏi về bộ ria mép]
    { stopping:
      - Bộ ria đó... là hàng giả đúng không? #speaker:Nam
        ... #speaker:"Quản gia"
        ...... #speaker:"Quản gia"
        Em bé. #speaker:"Quản gia"
        Hả- #speaker:Nam
        Em bé cái mồm thôi. #speaker:"Quản gia"
      - Im lặng nào, "cậu chủ nhỏ" của ta. #speaker:"Quản gia"
    }
    -> DONE
  * {doubt} [Hỏi tuổi]
    Khoan đã, anh bạn có đủ tuổi để đi làm không vậy? #speaker:Nam
    ... #speaker:"Quản gia"
    Có những thứ cậu không nên tìm hiểu đâu, chàng trai trẻ ạ. #speaker:"Quản gia"
    Biết hỏi trực tiếp là vô dụng, Nam quyết định lờ đi.
    -> DONE
  + [Bỏ đi]
    Nếu cậu có thắc mắc, tôi sẽ sẵn lòng giải đáp cậu, chàng trai trẻ. #speaker:"Quản gia"
    -> DONE


// chapter 2 - hallway 
// ---------

=== ch2_h3_clock ===
{ stopping:
  - Đồng hồ điểm 8 giờ tối, sắp đến giờ đóng cửa.
    Nam và Phong là vị khách cuối cùng trong ngày.
  - Đồng hồ điểm 8 giờ tối.
}
-> DONE

=== ch2_h3_table2 ===
{ ch2_h3_table2:
-1:
    ...
    ...
    Sau một thời gian Nam để ý cái bàn.
    Nam nhận ra cái bàn được lau rất kỹ càng.
    Chắc được lau bởi nhân viên cực kỳ chu đáo. #speaker:Nam #sprite:nam_thinking
    -> DONE
-else:
    Một cái bàn sạch sẽ hoàn toàn bình thường. 
    -> DONE
}

VAR voucher = false

=== ch2_h3_npc_mai ===
{ once:
  - Ca cuối cùng là chị Mai với bộ trang phục hầu gái.
    Khác với hai người còn lại, phong thái của chị ấy toát lên vẻ kiêu sa của những người hầu gái thời Victoria.
}
- (ask) Chị có thể giúp gì cho em? #speaker:Mai
  * {voucher} [<color=\#844600>Hỏi về voucher</color>]
    -> secret_encounter
  + [Hỏi về trang phục]
    { stopping:
      - Tại sao đồng phục nhân viên ở đây lại phỏng theo phong cách hầu gái \(và quản gia\) vậy chị? #speaker:Nam
        Hừmmm... thực ra thì... #speaker:Mai
        Quán không có đồng phục, là nhân viên HỌ tự quyết định trang phục thôi. #speaker:Mai
        Nhưng... tại sao lại là hầu gái \(và quản gia\)??? #speaker:Nam
        ...Ừmmm, chắc do sở thích của họ giống chị :D #speaker:Mai
        ...Hả? Đơn giản vậy thôi ư? #speaker:Nam
        Em không hài lòng với phong cách này sao? #speaker:Mai
        À- Không phải như vậy, em thích lắm chứ- #speaker:Nam
        Vậy sao? Em thích là tốt rồi. #speaker:Mai
      - Sở thích của chị là trang phục hầu gái. #speaker:Mai
    }
    -> ask
  + [Hỏi về người quản gia]
    { stopping:
      - Cậu ta tuy còn trẻ nhưng rất đáng tin cậy, là "quản gia" duy nhất trong trong quán. #speaker:Mai
        Tuổi của cậu ta ư? Ừmm... chị cũng không rõ. #speaker:Mai
        Được việc là được :D #speaker:Mai
      - Bộ ria mép của cậu ta... hình như là hàng giả đúng không? #speaker:Mai
    }
    -> ask
  + [Hỏi về cô hầu gái]
    { stopping: 
      - Em ấy là hầu gái có thái độ làm việc cực kì chăm chỉ. #speaker:Mai
        Mọi công việc được giao luôn được em ấy hoàn thành xuất sắc. #speaker:Mai
        Có điều... chị chưa bao giờ thực sự thấy em ấy làm việc. #speaker:Mai
      - Khoan đã, lần cuối mình thấy em ấy cầm chổi là khi nào nhỉ? #speaker:Mai 
    }
    -> ask
  + [Bỏ đi]
    -> leave
- (leave) { once: 
  - Chị biết em và Phong đang tập trung hết sức vào đồ án. #speaker:Mai
    Nhưng nhớ phải nghỉ ngơi giữ gìn sức khoẻ nghen. #speaker:Mai
    Chị lo cho hai "chủ nhân" của chị lắm đấy. #speaker:Mai
}
  Nếu em cần hỗ trợ, đừng ngại hỏi chị nhé. #speaker:Mai
  -> DONE

= secret_encounter
Làm sao mà em có được voucher này vậy? #speaker:Mai
Ừmm... em tìm thấy nó ở thư viện, nhờ vậy mà em cũng biết tới quán của chị. #speaker:Nam
Vậy sao? Có điều... voucher này hết hạn từ lâu rồi em ạ. #speaker:Mai
À-à vậy hả chị? Tiếc thật chứ- #speaker:Nam
Không sao, chỉ lần này thôi, chị sẽ đặc cách cho hai "chủ nhân nhỏ" của chị. #speaker:Mai
"Chủ nhân" muốn gọi gì nào? #speaker:Mai
* [Bạc xỉu không sữa] 2 bạc xỉu không sữa ạ. #speaker:Nam
* [Cacao nóng nhiều đá] 2 cacao nóng nhiều đá ạ. #speaker:Nam
* [Bánh sừng tê giác] 2 bánh sừng tê giác ạ. #speaker:Nam
- Chủ nhân vui lòng chờ ít phút nhé. #speaker:Mai
-> DONE

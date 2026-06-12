// chapter 2 - lobby
// ---------

=== ch2_l1_bed ===
{ stopping:
  - Chiếc giường trông thật bừa bộn.
    Tuy nhiên đó là lẽ thường tình với bất cứ ai sống một mình, kể cả Nam.
    Mình sẽ dọn giường sau vậy. #speaker:Nam #nam_thinking
  - Để sau vậy #speaker:Nam #nam_thinking
}
-> DONE


=== ch2_l1_papers ===
{ once:
  - Trên bàn ngổn ngang nhiều loại giấy tờ, một số còn rải rác lộn xộn dưới sàn.
    Toàn bộ là tài liệu Nam tổng hợp được trong mấy tuần qua phục vụ cho đồ án mà cả hai đang làm.
}
Nhìn bừa bộn quá, mình phải sắp xếp lại sau thôi. #speaker:Nam #nam_thinking
-> DONE


=== ch2_l1_window ===
{ stopping:
  - Chiếu qua cửa sổ là ánh nắng của buổi sáng mang theo sức sống của cảnh vật.
    Tiếng chim hót líu lo cùng tiếng gió lao xao qua kẽ lá tạo nên khúc âm hưởng của một buổi sáng sớm.
    ...Đâu đó có tiếng cãi cọ của hàng xóm vang vọng khắp khu nhà trọ.
    Mới sáng sớm mà họ đã om sòm thế nhỉ. #speaker:Nam #nam_thinking
  - Chắc mai mốt mình phải lắp cách âm vào mới được. #speaker:Nam #nam_thinking
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
    Nếu bạn thấy <voffset=15px><rotate="20">vật thể</voffset></rotate> đang lơ lửng.
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
{ talked: Cậu muốn gì? #speaker:"Chị" hầu gái|Lần đầu tiên trong đời, ngoại trừ mẹ cậu, Nam chủ động bắt chuyện với con gái.}
  + [Nói về vết đổ nước trên sàn]
    { stopping:
      - Ừm... {talked: Em|Mình} lỡ làm đổ nước lên sàn, {talked: chị|cậu} có thể cho {talked: em|tôi} mượn khăn lau... hay đại loại gì đó được không? #speaker:Nam #nam_thinking
        Nàng hầu gái trầm ngâm một lúc.
        Cậu cứ để đó đi, chốc nữa tôi sẽ dọn cho. #speaker:{talked: "Chị" hầu gái|Hầu gái}
        Vậy sao, {talked: em|} c-cảm ơn {talked: chị|cậu} nhiều. #speaker:Nam
      - Đừng có hối thúc tôi. #speaker:"Chị" hầu gái
    }
  + [Nói về bàn ghế bị lỗi toạ độ]
    { stopping:
      - Ừm... Hình như bàn ghế ở góc có gì đó không đúng thì phải. #speaker:Nam
        Nàng hầu gái trầm ngâm một lúc.
        À, khu đó khách vừa mới ra về. #speaker:{talked: "Chị" hầu gái|Hầu gái}
        ...Khoan đã, cậu đang sai khiến tôi đi dọn bàn đấy à? #speaker:{talked: "Chị" hầu gái|Hầu gái}
        Cậu nghĩ cậu là ai vậy hả?? #speaker:{talked: "Chị" hầu gái|Hầu gái}
        Ơ khoan đã, {talked: chị|cậu} hiểu lầm rồi, {talked: em|tôi} đâu có ý đó- #speaker:Nam
        Nhà ngươi đừng tưởng làm khách mà tỏ ra thượng đế nhé! #speaker:{talked: "Chị" hầu gái|Hầu gái}
        Cậu không nói thì tôi cũng sẽ dọn thôi! #speaker:{talked: "Chị" hầu gái|Hầu gái}
      - Đừng có hối thúc tôi. #speaker:"Chị" hầu gái
    }
  + [Bỏ đi]
    { talked: Nam không dám lên tiếng.|Có lẽ cậu vẫn chưa sẵn sàng.}
    -> DONE
- (talked) { once: 
  - Còn nữa... #speaker:Hầu gái
    S-sao vậy? #speaker:Nam
    Đừng có mà tuỳ tiện xưng hô "cậu" với tôi, cậu phải gọi tôi là chị đấy! #speaker:"Chị" hầu gái
    H-hả, à dạ vâng chị! #speaker:Nam
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
  - Nhân viên ca hiện tại là một thiếu niên khá trẻ tuổi, với phong cách trang phục bồi bàn lịch lãm kiểu Tây Âu.
    Vì lý do gì đó, Nam cảm thấy không thoải mái với bộ ria mép của cậu ta.
}
- (ask) Tôi có thể giúp được gì cho cậu, chàng trai trẻ? #speaker:"Quản gia"
  * [Hỏi về tuổi cậu ta]
    C-chàng trai trẻ ư? Tôi khá chắc cậu kém tuổi tôi- #speaker:Nam
    Thôi nào, chúng ta đều là đàn ông với nhau cả mà đúng không? #speaker:"Quản gia"
    Khoảng cách tuổi tác cũng không phải là điều quá quan trọng. #speaker:"Quản gia"
    Có lẽ kinh nghiệm và trải nghiệm mới là thứ thể hiện bản sắc riêng của mỗi con người chúng ta. #speaker:"Quản gia"
    Tôi tin chắc cậu cũng có cùng suy nghĩ mà đúng không?  #speaker:"Quản gia"
    Nam như cảm thấy khó xử, cậu chỉ bối rối gật đầu đồng tình. #speaker:Nam
    -> ask
  + [Hỏi về bộ ria mép]
    { stopping:
      - Bộ ria đó... là hàng giả đúng không? #speaker:Nam
        Haha, cậu có con mắt quan sát tinh tường đấy, chàng trai trẻ à. #speaker:"Quản gia"
        Tuy nhiên có những thứ cậu không nên hỏi trực tiếp, như vậy là rất khiếm nhã đấy. #speaker:"Quản gia"
        Phiền cậu hãy giữ bí mật giùm tôi nhé. #speaker:"Quản gia"
        Nam như bối rối không biết trả lời sao, cậu chỉ lúng túng gật đầu.
      - Bí mật này chỉ hai ta biết thôi nhé? #speaker:"Quản gia"
    }
    -> ask
  + [Hỏi về hầu gái ca trước]
    { stopping: 
      - Cô gái ca trước hả? Chà... Có thể nói cô ấy có tính cách hơi bốc đồng và khó gần. #speaker:"Quản gia"
        Nhưng ngược lại, cô ấy lại là người chịu khó và cần mẫn, luôn để ý mọi người xung quanh. #speaker:"Quản gia"
        Cậu hãy thử nói chuyện với cô ấy xem, biết đâu cô ấy lại mở lòng hơn thì sao. #speaker:"Quản gia"
        Lửa gần rơm lâu ngày cũng bén, chẳng phải sao? #speaker:"Quản gia"
      - Tôi nhớ không lầm thì tính cách đó gọi là... "tsundere" thì phải. #speaker:"Quản gia"
    }
    -> ask
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

=== ch2_h3_table1 ===
{ ch2_h3_table1:
-1:
    Trong Nam thoáng qua một suy nghĩ...
    Bàn ghế được sắp xếp rất gọn trái lại với vẻ kỳ lạ buổi sáng.
    ->DONE
-else:
    Quán vắng vẻ thật đấy. #speaker:Nam #sprite:nam_thinking
    ->DONE
}

=== ch2_h3_table2 ===
{ ch2_h3_table2:
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

VAR voucher = false

=== ch2_h3_npc_mai ===
{ once:
  - Ca cuối cùng là chị Mai với bộ trang phục hầu gái.
    Khác với hai người còn lại, phong thái của chị ấy toát lên vẻ kiêu sa của những người hầu gái thời Victoria.
}
- (ask) Chị có thể giúp gì cho em nào? #speaker:Mai
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
        Nam như lỡ một nhịp, không biết nói gì...
      - Sở thích của chị là trang phục hầu gái. #speaker:Mai
    }
    -> ask
  + [Hỏi về "quản gia"]
    { stopping:
      - Cậu ta tuy còn trẻ nhưng rất đáng tin cậy, cũng là "quản gia" duy nhất trong trong quán. #speaker:Mai
        Tuổi của cậu ta ư? Ừmm... chị cũng không rõ. #speaker:Mai
        Tại thấy được việc nên chị cứ vớ đại ấy mà, haha :D #speaker:Mai
      - Bộ ria mép đó... hình như là hàng giả đúng không? #speaker:Mai
    }
    -> ask
  + [Hỏi về "chị" hầu gái]
    { stopping: 
      - Em ấy luôn là nhân viên xuất sắc với thái độ làm việc chăm chỉ. #speaker:Mai
        Chỉ là... Em ấy hơi nhạy cảm trong việc ứng xử với mọi người. #speaker:Mai
        Đặc biệt là về khoản tuổi tác. #speaker:Mai
      - Chị luôn muốn có một đứa em gái như em ấy. #speaker:Mai
        Nhưng em ấy không thích bị người khác coi là trẻ con đâu. #speaker:Mai
        Đó có phải là "moe-gap" người ta hay đồn đại không nhỉ? #speaker:Mai
      - Chị luôn muốn có một đứa em gái như em ấy. #speaker:Mai
    }
    -> ask
  + [Bỏ đi]
    -> leave
- (leave) { once: 
  - Chị biết em và Phong đang tập trung hết sức vào đồ án. #speaker:Mai
    Nhưng nhớ phải nghỉ ngơi giữ gìn sức khoẻ nghen. #speaker:Mai
    Chị lo cho hai "chủ nhân nhỏ" của chị lắm đấy. #speaker:Mai
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
- Vâng, chủ nhân vui lòng chờ ít phút nhé. #speaker:Mai
-> DONE

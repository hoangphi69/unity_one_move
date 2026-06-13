// chapter 1 - lobby
// ---------

=== ch1_l1_bed ===
{!Chiếc futon cũ của chủ phòng trước để lại cho Nam.}
{ sleep: -> leave }
Chợp mắt một chút?
+ (sleep) [Đánh một giấc]
  Chiếc giường như cố gắng thôi thúc bạn đi ngủ.
  Tuy nhiên tầm này mà ngủ thì trưa mai bạn mới dậy nổi.
  -> leave
+ [Bỏ đi]
  -> leave
- (leave) Mình nên đi ra ngoài thì hơn #speaker:Nam #nam_thinking
  -> DONE


=== ch1_l1_laptop ===
{ once:
  - Chiếc laptop cũ mà Nam đã dành dụm cả 12 năm học để mua.
    Nó đáp ứng được hầu hết các công việc nhẹ nhàng...
    Bao gồm cả đánh LOL, bắn Volarant, v.v.
}
{ play_game: -> leave }
Tiếp tục chơi game?
+ (play_game) [Tiếp tục trận game]
  Khoan đã nào.
  Không phải bạn có việc quan trọng hơn đó là đi ra ngoài chạm cỏ sao?
  -> leave
+ [Bỏ đi]
  -> leave
- (leave) Mình nên đi ra ngoài thì hơn #speaker:Nam #nam_thinking
  -> DONE


=== ch1_l1_window ===
{ once:
  - Ánh nắng ban mai khẽ chiếu qua khung cửa sổ.
    Tán lá khẽ đung đưa dưới ngọn gió đầu chiều.
    Khung cảnh bên ngoài trông thật thơ và trữ tình...
    ...Nếu như không có cảnh hàng xóm đang cãi nhau.
}
Có lẽ mình nên lắp cái rèm vào. #speaker:Nam #nam_thinking
-> DONE


=== ch1_l1_boxes ===
{ once:
  - Nhiều loại thùng giấy được đặt ngổn ngang trong góc phòng.
    Chúng đa phần là các món hàng mà cậu đã từng thức đêm săn sale.
}
Nam tự nhủ sẽ dọn dẹp đống thùng giấy trong tương lai.
-> DONE


// chapter 1 - hallway 1
// ---------

=== ch1_h1_bookshelf ===
{ once:
  - Rất nhiều các cuốn sách bán chạy top \#1 được trưng bày ngay ngắn trên kệ.
    Đa số đều là các loại sách self-help mà Nam đã từng nghe qua.
}
Chọn một cuốn sách?
+ [Tắc Nhân Tâm]
  "<i>Cẩm nang sống vô tri, tắt chế độ đồng cảm, mặc kệ sự đời.</i>"
+ [Dạy Con Làm Màu 3]
  "<i>Hành trình trở thành tổng tài sống ảo, phông bạt cuộc đời từ hai bàn tay trắng. Phần 3</i>"
+ [Bỏ đi]
- { stopping:
  - Nghe nhảm nhí quá, ai lại viết ra mấy đầu sách này nhỉ? #speaker:Nam #sprite:nam_confused
  - Mấy cuốn sách này không phù hợp với mình. #speaker:Nam #sprite:nam_talk2
}
-> DONE


=== ch1_h1_locker ===
{ stopping: 
  - Một dãy các tủ cá nhân để bạn cất đồ của mình trước khi vào thư viện.
    Tuy nhiên do thư viện không đánh số tủ nên rất nhiều người đã gặp khó khăn trong việc tìm đồ của mình, trong đó có Nam.
    Nhận thức được sự hạn chế của trí nhớ ngắn hạn của bản thân, cậu tự nhủ sẽ không bao giờ dùng tủ cá nhân của thư viện.
  - ... #speaker:Nam #sprite:nam_talk2
    Khoan đã, đồ của mình ở tủ nào nhỉ? #speaker:Nam #sprite:nam_surprise
    ...À, hôm nay mình không đem đồ cá nhân lên thư viện. #speaker:Nam #sprite:nam_confused
  - Mình không đem đồ cá nhân lên thư viện. #speaker:Nam #sprite:talk1
}
-> DONE


=== ch1_h1_npc_confused ===
Balo của mình cất ở tủ nào nhỉ??? #speaker:Gái mất đồ
-> DONE


// chapter 1 - hallway 2
// ---------

=== ch1_h2_bookshelf1 ===
{ stopping:
  - Khu vực này bao gồm các loại sách giáo khoa, giáo trình và ngoại ngữ phổ thông.
    Tuy nhiên trình độ học vấn của cậu đã vượt xa các kiến thức mà những cuốn sách này có thể đem lại.
    Ít nhất là cậu nghĩ thế.
    Ý tưởng không có ở đây rồi #speaker:Nam #sprite:nam_silent
  - Mình nên đi tìm tủ sách khác. #speaker:Nam #sprite:nam_confused
}
-> DONE


=== ch1_h2_bookshelf3 ===
{ stopping: 
  - Khu vực này bao gồm các loại sách văn học nước ngoài.
    Rất nhiều các tác phẩm và tiểu thuyết nổi tiếng của các quốc gia phương Tây và phương Đông được sắp xếp ngay ngắn trên tủ.
    Điều khiến Nam chú ý đó là một lượng lớn các tiểu thuyết ngắn Nhật bị thiếu trên kệ.
    Ý tưởng không có ở đây rồi. #speaker:Nam #sprite:nam_confused
  - Mình nên đi tìm tủ sách khác. #speaker:Nam #sprite:nam_confused
}
-> DONE

=== ch1_h2_npc_coding ===
{ once:
  - Cậu học sinh đang miệt mài gõ phím trước một chiếc máy tính cũ kỹ.
    Màn hình trước mặt cậu ta hiển thị những dòng code bị gạch chân đỏ lòm.
}
Có lẽ mình không nên làm phiền cậu ta. #speaker:Nam #sprite:nam_talk
-> DONE

=== ch1_h2_npc_drop_books ===
{ stopping:
  - Có hai cô gái đang luống cuống bên chồng sách lộn xộn dưới sàn.
    Có lẽ một trong hai người họ vừa làm đổ sách của thư viện.
    Vì ưu tiên việc tìm kiếm ý tưởng, Nam lờ đi như chưa thấy gì.
    Hoặc do cậu không dám chủ động nói chuyện với con gái.
  - Mình nên tìm kiếm ý tưởng thôi. #speaker:Nam #sprite:nam_silent
}
-> DONE

=== ch1_h2_npc_reading ===
{ stopping: 
  - Cậu học sinh có vẻ đang rất chăm chú đọc cuốn sách với tựa đề "Re:Load - Isekai Checkpoint kara hajimeru: Nightmare mode de HP mo sōbi mo nashi, kuria suru yuitsu no hōhō wa nandomo Game Over ni natte shinario o anki suru koto.".
    Nếu như không vì đầu truyện cậu đang đọc thì có lẽ cậu trông tri thức hơn rất nhiều.
  - Chắc còn lâu cậu mới đọc xong hết đống tiểu thuyết này.
}
-> DONE


// chapter 1 - hallway 3
// ---------
=== ch1_h3_npc_confused ===
Balo mình cũng không có ở đây. #speaker: Gái mất đồ
Rốt cuộc balo của mình cất đâu nhỉ??? #speaker:Gái mất đồ
-> DONE
// -- Created By Shahid
// -- 13-10-2015
$(document).ready(function () {
    var i = 0;
    $.ajax({
        url: "Notification_IndirectPO.aspx?Method=AllPendingPO", type: 'GET', success: function (result) {



            var count = 0;

            $($(result).find(".Transfers").find(".TransferCount")).each(function () {


                if ($(this).next().text() == 'PORequest') {
                    $("#PORequests").text($(this).text());
                }
                else
                    if ($(this).next().text() == 'TransferRequest') {
                        $("#TransferRequests").text($(this).text());


                    }

                count += parseInt($(this).text());
                i++;

               
            });



            


            $(".notification-number").text(count);
            if (count > 0) {
                $(".notification").show();
            }

        }
    });

    $(".notification").click(function () {

        $(".pharmacyNotifications, .pnOverlay").fadeIn();

    });
    setInterval(
        function () {

            $.ajax({
                url: "Notification.aspx?Method=NewNotificationTR", type: 'GET', success: function (result) {


                    $($(result).find(".notifications").find(".notify")).each(function () {

                        console.log($(this).find("#method").text());
                        if ($(this).find("#method").text() == 'InitiatedToMe') {

                            $(this).find("#InitiatedToMe").css("display", "block");
                            $("#TransferRequests").text(parseInt($("#TransferRequests").text()) + 1);
                        }
                        //else {
                        //    $(this).find("#AcceptedOfMe").css("display", "block");
                        //    $("#trYours").text(parseInt($("#trYours").text()) + 1);
                        //}


                        $(".NotificationsHolder").append($(this));

                        var totalCount = 0;
                        totalCount = parseInt($(".notification-number").text());
                        totalCount += 1;
                        $(".notification-number").text(totalCount);
                        var TID = parseInt($(this).find("#TID").text());

                        console.log(TID);

                        setTimeout(function () {
                            $.ajax({
                                url: "Notification.aspx?Method=SetSeen&TID=" + TID, type: 'GET', success: function (result) {

                                }
                            });
                            $(this).remove();
                        }, 15000);




                    });

                 


                    console.log($(result).find(".notifications").html());


                }
            });


        }, 25000
        );






    setInterval(
        function () {

            $.ajax({
                url: "Notification_IndirectPo.aspx?Method=NewNotificationPO", type: 'GET', success: function (result) {


                    $($(result).find(".notifications").find(".notify")).each(function () {

                       

                        
                        $("#PORequest").text(parseInt($("#PORequest").text()) + 1);
                       

                        $(".NotificationsHolder").append($(this));

                        var totalCount = 0;
                        totalCount = parseInt($(".notification-number").text());
                        totalCount += 1;
                        $(".notification-number").text(totalCount);
                        var OrderID = parseInt($(this).find("#OrderID").text());

                        console.log(OrderID);

                        setTimeout(function () {
                            $.ajax({
                                url: "Notification_IndirectPO.aspx?Method=SetSeenPO&OrderID=" + OrderID, type: 'GET', success: function (result) {

                                }
                            });
                            $(this).remove();
                        }, 14000);




                    });




                    console.log($(result).find(".notifications").html());


                }
            });


        }, 25000
        );



});
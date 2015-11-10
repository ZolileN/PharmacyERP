// -- Created By Shahid
// -- 13-10-2015
$(document).ready(function () {
    var i=0;
    $.ajax({
        url: "Notification.aspx?Method=AllPendingTR", type: 'GET', success: function (result) {

            
            
            var count = 0;

            $($(result).find(".Transfers").find(".TransferCount")).each(function () {

                if (i == 0) {
                    $("#trOthers").text($(this).text());
                }
                else if (i == 1) {
                   
                    $("#trYours").text($(this).text());
                }
                i++;

                count += parseInt($(this).text());
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
                            $("#trOthers").text(parseInt($("#trOthers").text())+1);
                        }
                        else {
                            $(this).find("#AcceptedOfMe").css("display", "block");
                            $("#trYours").text(parseInt($("#trYours").text()) + 1);
                        }
                        

                        $(".NotificationsHolder").append($(this));

                        var totalCount = 0;
                        totalCount = parseInt($(".notification-number").text());
                        totalCount += 1;
                        $(".notification-number").text(totalCount);
                        var TID= parseInt($(this).find("#TID").text());

                        console.log(TID);

                        setTimeout(function(){
                            $.ajax({
                                url: "Notification.aspx?Method=SetSeen&TID=" + TID, type: 'GET', success: function (result) {
                                
                                }
                            });
                            $(this).remove();
                        }, 15000);

                        


                    });

                    //var totalCount = 0;
                    //totalCount = parseInt($(".notification-number").text());
                    //totalCount += 1;
                    //$(".notification-number").text(totalCount);

                    //var TID= parseInt($("#TID").text());

                    //console.log(TID);

                    //setInterval(function(){
                    //    $.ajax({
                    //        url: "Notification.aspx?Method=SetSeen&TID=" + TID, type: 'GET', success: function (result) {

                    //        }
                    //    });
                    //}, 45000);


                    console.log($(result).find(".notifications").html());
                   

                }
            });


        }, 25000
        );

    

});
$(document).ready(function () {
    $('#show_post_map').slideUp(300);
    $('#GridArea').slideDown(300);
    var swipecount = 0;
    $("#top_btn_hot").on("", function(){
        $("#top_btn_hot").attr("style","border-bottom-style: 1px;border-color:#FFAC55;");
    });
    $("#home").on("swipeleft", function () {
        swipecount += 1;
        alert(swipecount);
        switch (swipecount) {
            case 1:
                $("#top_btn_hot").click();
                break;
            case 2:
                $("#top_btn_share").click();
                break;
            case 3:
                $("#top_btn_emergency").click();
                break;
            case 4:
                $("#top_btn_together").click();
                break;
            case 5:
                $("#top_btn_sad").click();
                break;
        }
    });

    $('.toggle-btn').on("vclick", function () {
        console.log("toggle");
        $('.filter-btn').toggleClass('open');
        $('.one').attr("style", "display:block;")
        $('.two').attr("style", "display:block;")
    });

    //$('.filter-btn a').click(function () {
    //    console.log("filter");
    //    $('.filter-btn').removeClass('open');
    //});

    $('.one').click(function () {
        console.log("map");
        $('.filter-btn').removeClass('open');
        $('#GridArea').slideUp(300);
        $('#show_post_map').slideDown(300);
        $('.one').attr("style", "display:none;")
        $('.two').attr("style", "display:none;")
    });

    $('.two').click(function () {
        console.log("list");
        $('.filter-btn').removeClass('open');
        $('#show_post_map').slideUp(300);
        $('#GridArea').slideDown(300);
        $('.one').attr("style", "display:none;")
        $('.two').attr("style", "display:none;")
    });
});

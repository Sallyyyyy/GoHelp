$(document).ready(function () {
    kendo.culture("zh-TW");
    //grid
    $.ajax({
        type: "POST",
        url: "/Library/Index",
        dataType: "json",
        success: function (response) {
            console.log("in index");
            $("#show_post_grid").kendoGrid({
                dataSource: response,
                height: 550,
                pageable: false,
                columns: [
                    {
                        field: "Kind",
                        title: "種類",
                        width: "10%",
                        template: kendo.template($("#Kind-template").html())
                    },
                    {
                        field: "Nickname",
                        title: "暱稱",
                        width: "20%",
                        minScreenWidth: 750
                    }, {
                        field: "PostTitle",
                        title: "貼文名稱",
                        width: "30%"
                    }, {
                        field: "MeetAddress",
                        title: "地點",
                        width: "15%",
                        template: kendo.template($("#MeetAddress-template").html())
                    }, {
                        field: "StartTime",
                        format: "{0: yyyy-MM-dd}",
                        title: "時間",
                        minScreenWidth: 750
                    },
                    { command: { text: "查看", click: showDeleteDetail }, title: " ", width: "15%" }]
            });
        },
        error: function (error) {
            console.log(error);
        }
    });
    //search
    $("#insert_q").click(function () {
        $.ajax({
            type: "POST",
            url: "/Library/Insert",
            data: $("#ask_form").serialize(),
            dataType: "json",
            success: function (response) {
                var grid = $('#grid').data("kendoGrid");
                grid.setDataSource(response);
            },
            error: function (error) {
                console.log(error);
            }
        });
        return false;
    });
    //下拉式選單(類別名稱)
    //$.ajax({
    //    type: "POST",
    //    url: "/Library/ClassDropDown",
    //    data: $("#BookForm").serialize(),
    //    dataType: "json",
    //    success: function (response) {
    //        $("#classNameDrop,#insertClassDrop").kendoDropDownList({
    //            dataSource: response,
    //            dataTextField: "BookClassName",
    //            dataValueField: "BookClassName",
    //            optionLabel: "請選擇..."
    //        });
    //        $("#insertClassDrop").kendoDropDownList({
    //            dataSource: response,
    //            dataTextField: "BookClassName",
    //            dataValueField: "BookClassId",
    //            optionLabel: "請選擇..."
    //        });
    //    },
    //    error: function (error) {
    //        console.log(error);
    //    }
    //});
    ////下拉式選單(狀態)
    //$.ajax({
    //    type: "POST",
    //    url: "/Library/StatusDropDown",
    //    data: $("#BookForm").serialize(),
    //    dataType: "json",
    //    success: function (response) {
    //        $("#bookStatusDrop,#insertStatusDrop").kendoDropDownList({
    //            dataSource: response,
    //            dataTextField: "BookStatusName",
    //            dataValueField: "BookStatus",
    //            optionLabel: "請選擇..."
    //        });
    //    },
    //    error: function (error) {
    //        console.log(error);
    //    }
    //});
    ////下拉式選單(借閱人)
    //$.ajax({
    //    type: "POST",
    //    url: "/Library/KeeperDropDown",
    //    data: $("#BookForm").serialize(),
    //    dataType: "json",
    //    success: function (response) {
    //        $("#bookKeeperDrop").kendoDropDownList({
    //            dataSource: response,
    //            dataTextField: "BookKeeperName",
    //            dataValueField: "BookKeeperName",
    //            optionLabel: "請選擇..."
    //        });
    //    },
    //    error: function (error) {
    //        console.log(error);
    //    }
    //});
    //清除表單按鈕
    $("#clear").click(function () {
        alert("表單清除");
        $("#BookForm")[0].reset();
    });
    //新增視窗
    $('#add_book').click(function () {
        $("#showInsert").kendoWindow({
            actions: ["Pin", "Maximize", "Minimize", "Close"],
            title: "新增書籍",
            resizable: false,
            width: "700px"
        }).data("kendoWindow").center().open();
        $("#InsertForm").attr("style", "display:block");
        $("#insertStatusDrop").data("kendoDropDownList").enable();
    });
    $("#boughtDate").kendoDatePicker({
        format: "yyyy-MM-dd",
        parseFormats: ["yyyy/MM/dd", "yyyyMMdd"],
        dateInput: true
    });
    //驗證
    var validator = $("#book_form").kendoValidator({
        messages: {
            required: "此欄位為必填"
        }
    }).data("kendoValidator");
    //Insert
    $("#insert_btn").click(function () {
        //不曉得為何加了判斷認證就會跳轉頁面
        //if (validator.validate()) {
        $.ajax({
            type: "POST",
            url: "/Library/Insert",
            data: $("#InsertForm").serialize(),
            dataType: "json",
            success: function (response) {
                console.log("insert");
                alert("新增成功");
                $("#showInsert").data("kendoWindow").close();
                var grid = $('#grid').data("kendoGrid");
                grid.refresh();
            },
            error: function (error) {
                console.log(error);
            }
        });
        return false;
        //}
    });

    //刪除
    function showDeleteDetail() {
        console.log("showDeleteDetail");
        $("#hide_helping").click();
    }
});

function Dropdown_category() {
    $(".input-group .dropdown-menu").toggleClass('show');
}
function Toggle_MenuMobile() {
    $("#menu-mobile").show();
    $("#menu-mobile").addClass('show');
    $("#menu-mobile").fadeIn();
}
function CloseMenuMobile() {
    $("#menu-mobile").hide();
    $("#menu-mobile").removeClass('show');
}

$("#formSendEmail").on("submit", function (e) {
    e.preventDefault();
    $.post("/Home/Subscribe", $(this).serialize(), function(data) {
        if (data === "True") {
            alert("Cảm ơn bạn đã gửi thông tin cho chúng tôi!!");
            window.location.reload();
        } else {
            alert("Quá trình thực hiện không thành công. Vui lòng thử lại");
        }
    });
})
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(".password-field").each((i, obj) => {
    let eye =$("<i></i>").addClass("eye").addClass("bi-eye-fill")
        .on("click", (e) => {
            if ($(e.target).hasClass("bi-eye-fill")){
                $(e.target).removeClass("bi-eye-fill").addClass("bi-eye-slash-fill");
                $(obj).attr("type","text");
            }else{
                $(e.target).removeClass("bi-eye-slash-fill").addClass("bi-eye-fill");
                $(obj).attr("type","password");
            }

        })
    $(obj).wrap($("<div></div>").addClass("password-wrapper")).after(eye)
})

$(document).ready(function () {
    const closeInfoBanner = () => {
        $(".info-banner").addClass("hide")
        setTimeout(() => {
            $(".info-banner").hide()
        }, 200)
    }

    setTimeout(closeInfoBanner, 5000)
})

$('.cstm-hr:last').hide();

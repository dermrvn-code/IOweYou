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

$(function () {
    $('[data-toggle="tooltip"]').tooltip()
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



const userField = $("input[name=UserToSendTo]")
const amountField = $("input[name=Value]")
const currencyField = $("[name=Currency]")

const updateDisplay = () => {
    const info = $(".new-balance-info");

    let username = userField.val();
    let amount = amountField.val();
    let currency = currencyField.val();


    if (username && amount && currency){
        $.ajax({
            url: "/Search/BalanceWithPartner",
            type: "GET",
            data: { partnerName: username, currency: currency },
            success: function (response) {

                const setNewInfo = (username, newBalance, currency) => {
                    let newBalanceObj = $("<span></span>").text(newBalance).css("color", newBalance < 0 ? "red" : "green")
                    info.empty()
                    info.append("Your new balance with ", username, " will be ", newBalanceObj, " ", currency);
                    info.show();
                }

                if (amount < 0 || response.length === 0) {
                    info.hide();
                    return;
                }

                if (response === "NotYet"){
                    info.hide();
                }else{
                    let current = response["amount"];
                    setNewInfo(username, (current-parseInt(amount)), currency)
                }
            }
        });
    }

}

userField.on("keyup", updateDisplay)
amountField.on("keyup", updateDisplay)
currencyField.on("change", updateDisplay)

updateDisplay();

$('.cstm-hr:last').hide();

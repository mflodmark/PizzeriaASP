//$(document).ready(function() {
//    $(window).scroll(function() {
//        var scroll = $(window).scrollTop();
//        if (scroll > 50) {
//            $("#navbar").css("background-color", "white");
//            $("#navbar a").css("color", "#404040");
//            console.log("Scroll works");
//            navbarHover();
//        } else {
//            $("#navbar").css("background-color", "");
//            $("#navbar a").css("color", "white");
//            $("#cart-navbar a").css("color", "#404040");
//            console.log("Scroll at top");
//            navbarHover();
//        }
//    });
//    navbarHover();
//});

//function navbarHover() {
//    $("#navbar a:hover").css("color", " #e67e22");
//    $("#navbar a:hover").css("background-color", "transparent");
    
//}

window.onscroll = () => {
    var nav = document.querySelector('#navbar');
    //if (this.scrollY <= 10) nav.className = ''; else nav.className = 'scroll';
    if (this.scrollY > 10) nav.classList.add("scroll");
};
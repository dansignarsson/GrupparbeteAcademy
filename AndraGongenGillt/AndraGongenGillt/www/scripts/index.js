// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in cordova-simulate or on Android devices/emulators: launch your app, set breakpoints, 
// and then run "window.location.reload()" in the JavaScript Console.
(function () {
    "use strict";

    document.addEventListener( 'deviceready', onDeviceReady.bind( this ), false );

    function onDeviceReady() {
        // Handle the Cordova pause and resume events
        document.addEventListener( 'pause', onPause.bind( this ), false );
        document.addEventListener( 'resume', onResume.bind( this ), false );    
        
        // TODO: Cordova has been loaded. Perform any initialization that requires Cordova here.
        var parentElement = document.getElementById('deviceready');
        var listeningElement = parentElement.querySelector('.listening');
        var receivedElement = parentElement.querySelector('.received');
        listeningElement.setAttribute('style', 'display:none;');
        receivedElement.setAttribute('style', 'display:block;');
    };

    function onPause() {
        // TODO: This application has been suspended. Save application state here.
    };

    function onResume() {
        // TODO: This application has been reactivated. Restore application state here.
    };

    
})();


function StartPageLoad()
{
    const sleep = (milliseconds) => {
        return new Promise(resolve => setTimeout(resolve, milliseconds))
    }
    sleep(1350).then(() => {

        var userId = localStorage.getItem('userId');

        if (userId != null) {
            var url = "Search.html";
            $(location).attr('href', url);
        }
        else {
            var url = "MataStart.html";
            $(location).attr('href', url);
        }

    })
}

function SignUp() {
    console.log("Signup form submitted");

    //result.ratings[i].comment = result.ratings[i].comment.replace(/</g, "&lt;").replace(/>/g, "&gt;");
   
    var Username = $("#SignUpUsername").val().trim().replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var FirstName = $("#FirstName").val().replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var Password = $("#SignUpPassword").val();
    var PasswordRepeat = $("#SignUpPasswordRepeat").val();

    var viewModel = { Username: Username, FirstName: FirstName, Password: PasswordRepeat, PasswordRepeat: PasswordRepeat }

    $.ajax({
        url: "https://shoppiq.azurewebsites.net/register",
        type: "POST",
        data: JSON.stringify(viewModel),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            if (result.success)
            {
                console.log("SignUp fungerade");    
                console.log(result);

                $("#UserWasCreatedDiv").html("<h6>Användare skapad!</h6>");

                const sleep = (milliseconds) => {
                    return new Promise(resolve => setTimeout(resolve, milliseconds))
                }
                sleep(2000).then(() => {

                    var url = "MataStart.html";
                    $(location).attr('href', url);
                })
            }
            else
            {
                $("#ErrorMessageDiv").html("<p>" + result.responseText + "</p><p>" + result.test + "</p>");

            }
        },
        error: function (result) {

            console.log("SignUp-anropet fungerade inte");
            $("#ErrorMessageDiv").html("<h6>" + result.responseText + "</h6>");

            console.log(result);
        }
    })
}

function LogIn() {
    console.log("Login form submitted");

    var Username = $("#Username").val().trim();
    var Password = $("#Password").val();

    var viewModel = { Username: Username, Password: Password}

    $.ajax({
        url: "https://shoppiq.azurewebsites.net/login",
        type: "POST",
        data: JSON.stringify(viewModel),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log("Login()-anropet fungerade");
            console.log(result);

            if (result.success)
            {
                localStorage.setItem('userId', result.userId);
                localStorage.setItem('firstName', result.firstName);
                var url = "Search.html";
                $(location).attr('href', url);
            }
            else if (!result.loginSucceeded)
            {
                $("#LoginErrorMessageDiv").html("<span>" + result.loginResponseText + "<span/>");
            }
        },
            
        error: function (result) {
            console.log("Login() fungerade inte");
            console.log(result);
        }
    })
}

function Logout() {
    console.log("Logout");

    var userId = localStorage.removeItem('userId');
    var userId = localStorage.removeItem('firstName');
    var url = "MataStart.html";
    $(location).attr('href', url);
}

function Search() {
    console.log("Search function");
   
    var stores = [];
    $.each($('input[name="StoreCheckBox"]:checked'), function () {
        stores.push($(this).val());
    });

    var id = $("#SearchInput").val();
    var data = { searchText: id, storeIds: stores };

    console.log(data);
    
    $.ajax({
        url: "https://shoppiq.azurewebsites.net/searchTotalResult/",
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) { xhr.setRequestHeader('X-Token', localStorage.getItem('userId'));},

        success: function (result) {
            
            console.log("Search fungerade");
            console.log(result);

            html = "";

            html += result.length +    " träffar";

            for (i = 0; i < result.length; i++)
            {

                if (result[i].rating == "NaN")
                {
                    result[i].rating = 0;
                }


                if (i % 2 == 0) {
                    html += "<div class='row'>"
                }

                html += '<a class="col w-50 p-3" href="ProductDetails.html?id=' + result[i].id + '"><div>' +
                    "<div class='SearchImageDiv'>";
                //html += '<div href="ProductDetails.html?id=' + result[i].id + '" class="col w-50 p-3">'

                if (result[i].imgUrl != null)
                {
                    html += "<img src=" + result[i].imgUrl + " class='img-fluid' />"
                } 
                else
                {
                    html += "<img src='https://bevent-rasch.se/wp-content/themes/br.se/bilder/produkt/ingen_bild_finns.jpg' class='img-fluid' />"
                }
                //<span>" + parseFloat(result[i].rating).toFixed(1) + "</span> <br />

                html += "</div><span class='stars' data-rating='" + result[i].rating + "'></span><span>(" + result[i].totalComments + ")</span>" + "<br /><span>"
                    + result[i].name + "</span><br />";

                if (result[i].price != null){
                    html += "<span>Ca " + result[i].price + "kr</span>";
                }
                    
                html += "</div></a>";
                
                if (i % 2 == 1)
                {
                    html +="<div class='w-100'></div></div>"
                }
            }

            if (result.length % 2 == 1)
            {
                html += "<div class='col w-50 p-3'></div>"
            }
            
            $("#SearchResultDiv").html(html);

            $(".stars").each(function (i) {
                var rating = $(this).attr("data-rating");
                $(this).html(getStars(rating));

            });
        },
        error: function (result) {
            console.log("Search fungerade inte");
            console.log(result);
        }
    })
}

function GetProductDetails(){
    console.log("GetProductDetails");
    
    var urlParams = new URLSearchParams(window.location.search);
    var myParam = urlParams.get('id');

    var viewModel = { id: myParam }; // {id:"6"}

    console.log(viewModel);
    console.log(myParam);

    $(".rateyo").rateYo({
        rating: "0",
        precision: 0,
        starWidth: "20px"
    });
    
    $.ajax({
        url: "https://shoppiq.azurewebsites.net/GetDetailsById/" + myParam,
        type: "GET",
        beforeSend: function (xhr) { xhr.setRequestHeader('X-Token', localStorage.getItem('userId')); },
        success: function (result) {
            console.log("Getproductdetails fungerade");
            console.log(result);

            if (result.avgRating == "NaN") {

                result.avgRating = 0;
            }
            if (result.description == null) {

                result.description = result.name;
            }

            var html =
                "<h1>" + result.name + "</h1>" + "<br />";

            if (result.imgUrl != null)
            {
                html += "<img src=" + result.imgUrl + " class='productDetailsImg' />"
            }
            else
            {
                html += "<img src='https://bevent-rasch.se/wp-content/themes/br.se/bilder/produkt/ingen_bild_finns.jpg' class='productDetailsImg' />"
            }

            html += "<br /><span class='stars' data-rating='" + result.avgRating + "'></span><span>(" + result.ratings.length + ")</span></b>   " +
                "</div ><br />" + result.description;
            if (result.price != null) {
                html += "<br />Ca " + result.price + " kr";
            }

                
            $("#ProductDetailsDiv").html(html);

            html = "";

            for (i = 0; i < result.ratings.length; i++) {

                result.ratings[i].comment = result.ratings[i].comment.replace(/</g, "&lt;").replace(/>/g, "&gt;");

                html +=

                    "<strong>" + result.ratings[i].firstName + "</strong><span></span><span class='stars' data-rating='" + result.ratings[i].rating + "'></span></b><br /></div>"
                + result.ratings[i].comment + "<br /><br />"
            }

            $("#CommentsDiv").html(html);

            $(".stars").each(function (i) {
                var rating = $(this).attr("data-rating");
                $(this).html(getStars(rating));
                
            });
        },
        error: function (result) {
            console.log("Getproductdetails fungerade inte");
            console.log(result);
        }
    })
}

function getStars(rating) {

    // Round to nearest half
    rating = Math.round(rating * 2) / 2;
    let output = [];

    // Append all the filled whole stars
    for (var i = rating; i >= 1; i--)
        output.push('<i class="fa fa-star" aria-hidden="true" style="color: gold;"></i>&nbsp;');

    // If there is a half a star, append it
    if (i == .5) output.push('<i class="fa fa-star-half-o" aria-hidden="true" style="color: gold;"></i>&nbsp;');

    // Fill the empty stars
    for (let i = (5 - rating); i >= 1; i--)
        output.push('<i class="fa fa-star-o" aria-hidden="true" style="color: gold;"></i>&nbsp;');

    return output.join('');

}

function Comment() {
    console.log("Comment submitted");

    var Rating = $(".rateyo").rateYo("rating");

    //var Rating = $("#userRating").val();
    var Comment = $("#comment").val().replace(/</g, "&lt;").replace(/>/g, "&gt;");

    var UserId = localStorage.getItem('userId');

    var urlParams = new URLSearchParams(window.location.search);
    var myParam = urlParams.get('id');

    var viewModel = { Rating: Rating, Comment: Comment, UserId: UserId, ProductId: myParam }

    $.ajax({
        url: "https://shoppiq.azurewebsites.net/setRating",
        type: "POST",
        data: JSON.stringify(viewModel),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) { xhr.setRequestHeader('X-Token', localStorage.getItem('userId')); },
        success: function (result) {
            console.log("Comment() fungerade");
            console.log(result);

            if (result.success)
            {
                var urlParams = new URLSearchParams(window.location.search);
                var myParam = urlParams.get('id');

                var url = "ProductDetails.html?id=" + myParam;
                $(location).attr('href', url);
            }
            else
            {
                $("#ErrorMessageDiv").html("<p>Du har redan lämnat ett omdöme för denna produkten, "
                    //+ 'klicka <button class ="btn" onclick="RemoveRating()">Här</button> för att ersätta den.</p>'
                    + '<p>Du kan ta bort din kommentar under <a id="ToMyPageLink" href="MyPage.html">Min sida</a></p>'
                );

            }

        },
        error: function (result) {
            console.log("Comment() fungerade inte");
            console.log(result);
        }
    })
}

var areStoresShowing = false;

function ShowStores() {
    console.log("Show stores clicked")

    $.ajax({
        url: "https://shoppiq.azurewebsites.net/ShowStores",
        type: "POST",
        beforeSend: function (xhr) { xhr.setRequestHeader('X-Token', localStorage.getItem('userId')); },
        success: function (result) {
            console.log("ShowStores-anropet fungerade");
            console.log(result);


            var html = "";
            if (!areStoresShowing) {

                for (i = 0; i < result.length; i++) {
                    html += "<label class='btn btn-info btn-store'> <input name='StoreCheckBox' type='checkbox' value='" + result[i].id + "' onclick='Search()'/> " + result[i].storeName + "</label>";

                    //if (i % 2 == 1)
                    //{
                    //    html += "<br />"
                    //}
                }
            }

            $("#storesDiv").html(html);
            areStoresShowing = !areStoresShowing; 
        },
        error: function (result) {
            console.log("ShowStores-anropet fungerade inte");
            console.log(result);
        }
    })
    
}

function CheckLoggedIn() {
    $.ajax({
        url: "https://shoppiq.azurewebsites.net/CheckLogin",
        type: "POST",
        data: JSON.stringify(localStorage.getItem('userId')),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result)
            {
                var url = "Search.html";
                $(location).attr('href', url);
            }
        },
        error: function (result) {
            console.log(result);
        }
    })

}

function validatePassword() {
    var password = document.getElementById("SignUpPassword")
        , confirm_password = document.getElementById("SignUpPasswordRepeat");

    if (password.value.length >= 6)
    {
        SignUpPassword.setCustomValidity('');
        if (/\d/.test(password.value))
        {
            SignUpPassword.setCustomValidity('');
            if (password.value != confirm_password.value)
            {
                SignUpPasswordRepeat.setCustomValidity("Lösenorden matchar ej");
            }
            else
            {
                SignUpPasswordRepeat.setCustomValidity('');
            }            
        }
        else
        {
            SignUpPassword.setCustomValidity("Lösenorden måste innehålla en siffra.");
        }        
    }
    else
    {
        SignUpPassword.setCustomValidity("Lösenorden måste ha minst 6 tecken.");
    }    
}

function GetUserInfo() {

    var UserId = localStorage.getItem('userId');

    var data = { UserID: UserId }

    GetEmail();

    $.ajax({
        url: "https://shoppiq.azurewebsites.net/myPage",
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) { xhr.setRequestHeader('X-Token', localStorage.getItem('userId')); },
        success: function (result) {
            console.log("GetUserInfo()-anropet fungerade");
            console.log(result);
            
            html = "";
            if (result == null) {
                html += "Du har inte lämnat några omdömen än"
            }
            else {
                for (i = 0; i < result.ratings.length; i++) {

                    html += ""

                    html += "<a href=ProductDetails.html?id=" + result.ratings[i].productID + "><div><strong>" + result.ratings[i].productName + "</strong><br />"
                        + "<img src='" + result.ratings[i].imgUrl + "' style='height:50px' /></div></a>"

                    html +=
                        "<strong>" + result.firstName + " </strong><span></span><span class='stars' data-rating='"
                        + result.ratings[i].userRating + "'></span></b><br /></div>" + result.ratings[i].userComment
                        + "<br /><button class='btn btn-light' id='btn-RemoveRating' onclick='RemoveRating(" + result.ratings[i].ratingID + ")'>Ta bort</button><br /><br />"
                }
            }

            $("#UserCommentsDiv").html(html);

            $(".stars").each(function (i) {
                var rating = $(this).attr("data-rating");
                $(this).html(getStars(rating));

            });
            
        },
        error: function (result) {
            console.log("GetUserInfo()-anropet fungerade inte");
            console.log(result);
        }
    })
}

function GetFirstNameFromLocalStorage(){

    $("#UserFirstNameDiv").html("<a href='MyPage.html'><li class='list-group-item'><b>"+
        localStorage.getItem('firstName') + "</b></li></a>");

}

function RemoveRating(ratingId){

    console.log(ratingId);
    
    var data = { RatingId: ratingId };

    console.log(data);

    $.ajax({
        url: "https://shoppiq.azurewebsites.net/deleteRating",
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) { xhr.setRequestHeader('X-Token', localStorage.getItem('userId')); },
        success: function (result) {

            console.log("DeleteComment()-anropet fungerade")
            console.log(result);

            var url = "MyPage.html";
            $(location).attr('href', url);

        },
        error: function (result) {
            console.log("DeleteComment()-anropet fungerade int")

        }
    })

}

function scrollToTop() {
    window.scrollTo(0,0);
}

function CheckLoggedInForAboutPage() {
    $.ajax({
        url: "https://shoppiq.azurewebsites.net/CheckLogin",
        type: "POST",
        data: JSON.stringify(localStorage.getItem('userId')),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            html = "";
            if (result) {
                

                html += "<div id='UserFirstNameDiv'></div>"
                    + '<a href="Search.html"><li class="list-group-item">Sök</li></a>'
                    + '<a href="MyPage.html"><li class="list-group-item">Min Sida</li></a>'
                    + '<a href="About.html"><li class="list-group-item">Om Shoppiq</li></a>'
                    + '<a href="#" onclick="Logout()"><li class="list-group-item">Log out</li></a>';
            }
            else
            {
                html += '<a href="MataStart.html"><li class="list-group-item">Logga in</li></a>';
            }

            $("#AboutHamburgerDiv").html(html);


        },
        error: function (result) {
            console.log(result);
        }
    })
}

function GetEmail() {

    var UserId = localStorage.getItem('userId');

    var data = { UserID: UserId }

    $.ajax({
        url: "https://shoppiq.azurewebsites.net/email",
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) { xhr.setRequestHeader('X-Token', localStorage.getItem('userId')); },
        success: function (result) {

            console.log(result);
            html = "";

            html += "<h4>" + localStorage.getItem("firstName") + "</h4><p> " + result.email + " </p>";

            $("#UserInfoDiv").html(html);
        },
        error: function (result) {
            console.log(result);
        }
    })
}
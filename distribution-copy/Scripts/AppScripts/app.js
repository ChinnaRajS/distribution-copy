$(window).scroll(function(){
            if ($(window).scrollTop() >= 10) {
                $('nav.bg-trans').addClass('bg-col');               
                
            }
            else {
                $('nav.bg-trans').removeClass('bg-col');             
                
            }
        });
function changeName()
{
	document.getElementById('desc').innerHTML = "Show Description"
	
}		
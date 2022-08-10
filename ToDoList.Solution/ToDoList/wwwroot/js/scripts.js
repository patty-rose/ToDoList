//business logic
function markCompleted(checkbox) {
  let form = checkbox.closest('form');
  form.submit();
}

$(document).ready(function() {

  $('.itemCheckbox').on('click', function (e) {
    e.preventDefault();
    console.log(e);
    console.log(e.target);
    markCompleted(e.target);
    console.log(e.target)

    
  });
});
$(document).ready(function () {
	$(document).on('click', '.deletebutton', function () {
		var info = $(this).parents('.container')[0];
		return confirm(`Delete ${$(info).find('.productinfo').text()}?`);
	}); 
});
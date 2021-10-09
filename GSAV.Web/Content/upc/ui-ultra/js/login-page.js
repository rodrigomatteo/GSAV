// LRN-121759 Make the login username/ password labels switch between being placeholder
// text for the inputs and floating above on focus to improve accessibility.
jQuery(function() {
  // Helper for adding/ removing the 'float-above' on focusin/ out.
  var setFocusEvents = function (input, ignoreFocusoutAction) {
    input.on('focusin', function (event) {
      jQuery(event.target).prev('label').addClass('float-above');
    });

    input.on('focusout',function (event) {
      if (ignoreFocusoutAction && ignoreFocusoutAction()) {
        return;
      }

      var target = jQuery(event.target);
      if (!target.val()) {
        target.prev('label').removeClass('float-above');
      }
    });
  }

  var usernameInput = jQuery('#user\_id');
  var passwordInput = jQuery('#password');

  // ignorePasswordFocusoutAction is used to overcome some issues with autfill.
  // In Chrome, the page password input doesn't report a value until the user reacts
  // with the page. This means we can't rely on .val() to float the password label
  // on autofill. We overcome this by automatically float both labels if the username
  // was autofilled and focusing the password input. However, when the user first
  // leaves the password field, the value is still not set. We set
  // ignorePasswordFocusoutAction to keep the password label floating to avoid
  // text overlapping.
  var ignorePasswordFocusoutAction = false;
  setFocusEvents(passwordInput, function () {
    if (ignorePasswordFocusoutAction) {
        ignorePasswordFocusoutAction = false;
        return true;
      }
      return false;
    });

    setFocusEvents(usernameInput, false);

    var prevUsername = '';
    var toggleFloatAboveStyleForLabel = function (hasText) {
      jQuery('#user\_id, #password').prev('label').toggleClass('float-above', hasText);
    }

    var onInputChanged = function (event) {
      // Update the input variables so get current values
      usernameInput = jQuery('#user\_id');
      passwordInput = jQuery('#password');

      // The input event is fired every time the users types/ changes text. To avoid
      // floating the labels every time a letter is typed/ removed, check if more
      // than 1 letter has changed and assume it's a copy/ paste or autofill.
      if (usernameInput.val().length - prevUsername.length <= 1) {
        prevUsername = usernameInput.val();
        return;
      }

      var hasText = !!usernameInput.val();
      if (hasText) {
        toggleFloatAboveStyleForLabel(hasText);
      }
      if (hasText && !passwordInput.val()) {
        ignorePasswordFocusoutAction = true;
      }
  };
  usernameInput.on('input', onInputChanged);

  function isUsernameInputWebkitAutofilled() {
    try {
      return usernameInput.is('input:-webkit-autofill');
    } catch { // Firefox and IE throw "unrecognized expression: unsupported pseudo: -webkit-autofill" errors
      return false;
    }
  }

  var checkIfThereWasAutoFill = setInterval(function(e){
    // In Chrome the autofill value is not available until the user starts interacting with the page
    // Since the value is not available we need to check that the webkit-autofill exists.
    // The solution is inspired by https://stackoverflow.com/a/35783761
    // In other browsers the value is available.
    if (isUsernameInputWebkitAutofilled()) {
      toggleFloatAboveStyleForLabel(true);
      clearInterval(checkIfThereWasAutoFill);
    }

    // We have the value in Edge. webkit-autofill does not exist when autofilled
    if (usernameInput.val()) {
      onInputChanged(e);
      clearInterval(checkIfThereWasAutoFill);
    }
  }, 30);
});

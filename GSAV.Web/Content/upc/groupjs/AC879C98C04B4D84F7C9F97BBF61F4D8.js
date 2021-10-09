function validate_form_no_challenge( form )
{
  form.encoded_pw.value = base64encode( form.password.value );
  form.encoded_pw_unicode.value = b64_unicode( form.password.value );
  form.password.value = "";
  return true;
}

function validate_form_with_challenge( form )
{
  var passwd_enc = hex_md5( form.password.value );
  var encoded_pw_unicode = calcMD5( form.password.value );
  var final_to_encode = passwd_enc + form.one_time_token.value;
  form.encoded_pw.value = hex_md5( final_to_encode );
  final_to_encode = encoded_pw_unicode + form.one_time_token.value;
  form.encoded_pw_unicode.value = calcMD5( final_to_encode );
  form.password.value = "";
  return true;
}

function validate_form( form, useChallengeResponse, skipEncoding )
{
  var loginErrorMessage = document.getElementById( "loginErrorMessage" );
  
  form.user_id.value = form.user_id.value.replace( /^\s*|\s*$/g, "" );
  if ( form.user_id.value == "" || form.password.value == "")
  {
    if ( loginErrorMessage )
    {
      loginErrorMessage.parentNode.removeChild( loginErrorMessage );
    }
    setTimeout( function() { alert( JS_RESOURCES.getString('validate.login.invalid.username.or.pass') ); }, 0 );
    if ( form.user_id.value == "" )
    {
      form.user_id.focus();
    }
    else
    {
      form.password.focus();
    }
    return false;
  }

  if ( !skipEncoding ) // Only challenge-response and b64 for legacy auth
  {
    if ( useChallengeResponse )
    {
      return validate_form_with_challenge( form );
    }
    else
    {
      return validate_form_no_challenge( form );
    }
  }
}

function verify_cookies_enabled()
{
  document.cookie = "cookies_enabled=yes";
  /* if ( !document.cookie )
  {
    document.location.href = "/webapps/login/nocookies.jsp";
  } */
  document.cookie = "cookies_enabled=yes;expires=Thu, 01-Jan-1970 00:00:01 GMT";
}

//verify_cookies_enabled();

function verify_username_password(form)
{
  var check = eventHandler(form, form.user_id, form.password);
  var delayedCheck = function()
  {
    // the pasted value can only be get after the onpaste event,
    // so instead of invoke the event handler directly, we need to put it in the next event process
    setTimeout( check, 0 );
  };

  form.user_id.onchange = check;
  form.user_id.onkeyup = check;
  form.user_id.onkeydown = check;
  form.user_id.onpaste = delayedCheck;

  form.password.onchange = check;
  form.password.onkeyup = check;
  form.password.onkeydown = check;
  form.password.onpaste = delayedCheck;
}

let isFirstEvent = true;
function eventHandler( form, username, password )
{
  return function(e)
  {
    var u = username.value;
    var p = password.value;
    var disabled = false;

    u = u.replace( /^\s*|\s*$/g, "" );

    if ( u == '' )
    {
      disabled = true;
    }
    // LRN-122924 browsers fire an onchange event when they autofill, but some don't
    // report the password field's value until the user interacts with the page.
    // To get around this, just assume if the username is filled on the first event
    // fired, the password is too and enable the sign in button.
    if ( p == '' && !isFirstEvent )
    {
      disabled = true;
    }

    isFirstEvent = false;
    form.login.disabled = disabled;
  }
}
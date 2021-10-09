// since this is used on both the login page and educator preview pages
// and the login page is served with prototype, these are used to determine
// how to select the body and add/ remove the class names.
var isJQueryLoaded = window.jQuery !== undefined;
var isPrototypeJSLoaded = window.Prototype !== undefined;

// show the focus outline if the user tabs
function keyHandler(event) {
  if (event.keyCode === 9) {
    if (isJQueryLoaded) {
      window.jQuery(document.body).removeClass('hide-focus-outline');
    } else if (isPrototypeJSLoaded) {
      window.jQuery(document.body).removeClassName('hide-focus-outline');
    }
  }
}

// hide the focus outline if the user clicks
function mouseHandler() {
  if (isJQueryLoaded) {
    window.jQuery(document.body).addClass('hide-focus-outline');
  } else if (isPrototypeJSLoaded) {
    window.jQuery(document.body).addClassName('hide-focus-outline');
  }
}

// make sure either jQuery or prototype JS are loaded so we won't
// get any console errors
if (isJQueryLoaded || isPrototypeJSLoaded) {
  document.addEventListener('keyup', keyHandler, true);
  document.addEventListener('keydown', keyHandler, true);
  document.addEventListener('mousedown', mouseHandler, true);
}
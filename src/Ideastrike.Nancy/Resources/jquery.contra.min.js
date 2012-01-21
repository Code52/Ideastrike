/* jQuery Contra v1.0
 * http://jonraasch.com/blog/jquery-contra-plugin
 *
 * Copyright (c) 2009 Jon Raasch (http://jonraasch.com/)
 * Licensed under the MIT License (see http://dev.jonraasch.com/contra/docs#licensing)
 */
(function(a){a.contra=function(e,d){function c(f){if(f==d.code[b]){b++;if(b>=d.code.length){e()}}else{b=0}}if(typeof e!="function"){return false}var b=0,d=d||{};d.scope=d.scope||a(document);d.code=d.code||[38,38,40,40,37,39,37,39,66,65,13];d.scope.keyup(function(f){c(f.which||f.keyCode)})};a.fn.contra=function(c,b){var b=b||{};b.scope=a(this);a.contra(c,b)}})(jQuery);
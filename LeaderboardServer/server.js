// Dependencies.
let express = require('express');
let bodyParser = require('body-parser');
let fs = require('fs');
var RateLimit = require('express-rate-limit');

//and create our instances
let app = express();
let router = express.Router();

app.enable('trust proxy');

var limiter = new RateLimit({
    windowMs: 15 * 60 * 1000, // 15 minutes
    max: 100, // limit each IP to 100 requests per windowMs
    delayMs: 0 // disable delaying - full speed until the max limit is reached
});

// set our port to either a predetermined port number
let port = process.env.API_PORT || 3001;

// now we should configure the API to use bodyParser and look for
// JSON data in the request body
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

// To prevent errors from Cross Origin Resource Sharing
app.use(function(req, res, next) {
  res.setHeader('Access-Control-Allow-Origin', '*');
  res.setHeader('Access-Control-Allow-Credentials', 'true');
  res.setHeader('Access-Control-Allow-Methods', 'GET,HEAD,OPTIONS,POST,PUT,DELETE');
  res.setHeader('Access-Control-Allow-Headers', 'Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers');
  // Remove cache for now.
  res.setHeader('Cache-Control', 'no-cache');
  next();
});
// now we can set the route path & initialize the API
router.get('/', function(req, response) {
  fs.readFile('index.html', function (err, data) {
      response.writeHead(200, {
          'Content-Type': 'text/html'
      });
      response.write(data);
      response.end();
  });
});

router.route('/scores')
    .get(function(request, response) {
        fs.readFile('scores.txt', function (err, data) {
            response.writeHead(200, {
                'Content-Type': 'text/plain'
            });
            response.write(data);
            response.end();
        });
    })
    .post(function(request, response) {
        let line = "\n"+request.body.username + ": " + request.body.score;
        fs.appendFile('scores.txt', line, function (err) {
            if (err) throw err;
            response.json({message: "Success"});
        });
    });


// Use our router configuration when we call /api
app.use('/ld41', router);
app.use(limiter);

//starts the server and listens for requests
app.listen(port, function() {
  console.log(`api running on port ${port}`);
});
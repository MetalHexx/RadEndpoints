import http from 'k6/http';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: true,
    vus: 1,
    duration: '10s'
};

export let payload = {
    firstName: 'John',
    lastName: 'Doe',
};

export default () => {    
    let url = 'http://localhost:5039/pure/1'; //pure minimal api
    // data_received..................: 234 kB 23 kB / s
    // data_sent......................: 168 kB 17 kB / s
    // http_req_blocked...............: avg = 275.48µs min = 0s      med = 253.8µs max = 5.99ms  p(90) = 498µs   p(95) = 501.29µs
    // http_req_connecting............: avg = 242.96µs min = 0s      med = 244.6µs max = 543.6µs p(90) = 487.7µs p(95) = 500.8µs
    // http_req_duration..............: avg = 11.56ms  min = 1.48ms  med = 14.99ms max = 15.4ms  p(90) = 15.28ms p(95) = 15.31ms
    //{ expected_response: true }...: avg = 11.56ms  min = 1.48ms  med = 14.99ms max = 15.4ms  p(90) = 15.28ms p(95) = 15.31ms
    // http_req_failed................: 0.00 %  ✓ 0         ✗ 841
    // http_req_receiving.............: avg = 213.09µs min = 0s      med = 246.2µs max = 1ms     p(90) = 431.5µs p(95) = 499µs
    // http_req_sending...............: avg = 13.17µs  min = 0s      med = 0s      max = 523.2µs p(90) = 0s      p(95) = 0s
    // http_req_tls_handshaking.......: avg = 0s       min = 0s      med = 0s      max = 0s      p(90) = 0s      p(95) = 0s
    // http_req_waiting...............: avg = 11.34ms  min = 996.8µs med = 14.99ms max = 15.14ms p(90) = 15.06ms p(95) = 15.08ms
    // http_reqs......................: 841    84.091597 / s
    // iteration_duration.............: avg = 11.89ms  min = 1.74ms  med = 15.3ms  max = 15.92ms p(90) = 15.67ms p(95) = 15.72ms
    // iterations.....................: 841    84.091597 / s
    // vus............................: 1      min = 1       max = 1
    // vus_max........................: 1      min = 1       max = 1

    //let url = 'http://localhost:5039/custom-examples/1'; //custom endpoint
    //data_received..................: 254 kB 25 kB / s
    // data_sent......................: 193 kB 19 kB / s
    // http_req_blocked...............: avg = 342.6µs  min = 0s      med = 287.7µs max = 6ms      p(90) = 521.1µs  p(95) = 732.16µs
    // http_req_connecting............: avg = 306.91µs min = 0s      med = 283µs   max = 1.73ms   p(90) = 501.4µs  p(95) = 526.45µs
    // http_req_duration..............: avg = 10.58ms  min = 1.5ms   med = 14.49ms max = 17.71ms  p(90) = 15.25ms  p(95) = 15.28ms
    //{ expected_response: true }...: avg = 10.58ms  min = 1.5ms   med = 14.49ms max = 17.71ms  p(90) = 15.25ms  p(95) = 15.28ms
    // http_req_failed................: 0.00 %  ✓ 0         ✗ 915
    // http_req_receiving.............: avg = 229.58µs min = 0s      med = 231µs   max = 1.01ms   p(90) = 489.64µs p(95) = 501µs
    // http_req_sending...............: avg = 34.2µs   min = 0s      med = 0s      max = 548.79µs p(90) = 0s       p(95) = 498.83µs
    // http_req_tls_handshaking.......: avg = 0s       min = 0s      med = 0s      max = 0s       p(90) = 0s       p(95) = 0s
    // http_req_waiting...............: avg = 10.31ms  min = 998.9µs med = 14.49ms max = 17.49ms  p(90) = 15ms     p(95) = 15.07ms
    // http_reqs......................: 915    91.380411 / s
    // iteration_duration.............: avg = 10.94ms  min = 1.96ms  med = 15.21ms max = 18.01ms  p(90) = 15.65ms  p(95) = 15.69ms
    // iterations.....................: 915    91.380411 / s
    // vus............................: 1      min = 1       max = 1
    // vus_max........................: 1      min = 1       max = 1
                    

    //let url = 'http://localhost:5039/examples/1'; //radEndpoint w/Mediator
    // data_received..................: 281 kB 28 kB / s
    // data_sent......................: 206 kB 21 kB / s
    // http_req_blocked...............: avg = 277.84µs min = 0s     med = 243.7µs  max = 6ms     p(90) = 499.9µs  p(95) = 501.96µs
    // http_req_connecting............: avg = 231.72µs min = 0s     med = 226.15µs max = 1.85ms  p(90) = 499.21µs p(95) = 500.9µs
    // http_req_duration..............: avg = 9.57ms   min = 1.99ms med = 11.79ms  max = 19.5ms  p(90) = 15.29ms  p(95) = 15.33ms
    //{ expected_response: true }...: avg = 9.57ms   min = 1.99ms med = 11.79ms  max = 19.5ms  p(90) = 15.29ms  p(95) = 15.33ms
    // http_req_failed................: 0.00 %  ✓ 0          ✗ 1010
    // http_req_receiving.............: avg = 234.51µs min = 0s     med = 261.44µs max = 1ms     p(90) = 499.21µs p(95) = 502.2µs
    // http_req_sending...............: avg = 31.88µs  min = 0s     med = 0s       max = 546.1µs p(90) = 0s       p(95) = 497.81µs
    // http_req_tls_handshaking.......: avg = 0s       min = 0s     med = 0s       max = 0s      p(90) = 0s       p(95) = 0s
    // http_req_waiting...............: avg = 9.3ms    min = 1.49ms med = 11.5ms   max = 19.5ms  p(90) = 15ms     p(95) = 15.08ms
    // http_reqs......................: 1010   100.943653 / s
    // iteration_duration.............: avg = 9.9ms    min = 2.29ms med = 12.1ms   max = 20.17ms p(90) = 15.66ms  p(95) = 15.72ms
    // iterations.....................: 1010   100.943653 / s
    // vus............................: 1      min = 1        max = 1
    // vus_max........................: 1      min = 1        max = 1


    let payload = JSON.stringify({
        firstName: 'Luke',
        lastName: 'Skywalker'
    });

    let params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    let response = http.put(url, payload, params);
}
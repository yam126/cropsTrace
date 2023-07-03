

//axios.defaults.baseURL = 'http://39.99.205.8:5001/';
//axios.defaults.baseURL = 'http://120.24.240.133:5001/';
axios.defaults.baseURL = 'http://localhost:5329/';


function axnormal(data) {

}
function axform() {
    axios.defaults.headers['Content-Type'] = 'application/x-www-form-urlencoded';
}
function axlogin(url, data) {
    return new Promise((resolve, reject) => {
        url = url + '?' + data;
        axios.post(url)
            .then(res => {
                resolve(res.data)

            })
            .catch(err => {
                console.log(err)
                reject(err.data)
            })
    })
}



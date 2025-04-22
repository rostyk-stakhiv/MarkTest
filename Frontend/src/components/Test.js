import axios from 'axios';
import React, { Component } from 'react';
export class Test extends Component {
  static displayName = Test.name;

  constructor(props) {
    super(props);
    this.state = {
      startDate: "",
      finishDate: "",
      name: "",
      numberOfAttempts: 0,
      difficulty: "3",
      maxMark: 0,
      mark:0,
      subject: "",
      duration: 0,
      copySuccess: "Скопіювати",
      invitationLink: "",
      receivers: ""
    }
  }
  setDifficulty(d) {
    this.setState({ difficulty: d })
  }
  setReceivers(d) {
    this.setState({ receivers: d })
  }
  copyToClipboard = (e) => {
    this.textArea.select();
    document.execCommand('copy');
    e.target.focus();
    this.setState({ copySuccess: 'Скопійовано!' });
  };
  startTest(e) {
    e.preventDefault();
    window.location.href +=`/passTest?difficulty=${this.state.difficulty}`
  }
  componentDidMount() {
    axios.get(`/Tests/${this.props.match.params.id}`, { headers: { "Authorization": `Bearer ${localStorage["token"]}` } })
      .then(response => {
        console.log(response); this.setState({
          startDate: new Date(response.data.startDate),
          finishDate: new Date(response.data.endDate),
          name: response.data.name,
          numberOfAttempts: response.data.numberOfAttempts,
          maxMark: response.data.maxMark,
          duration: response.data.durability,
          subject: response.data.subject,
          mark:response.data.mark,
          isOwner: (response.data.ownerId === parseInt(localStorage["UserID"]) && response.data.userId === parseInt(localStorage["UserID"]))
        })
      }).catch(error => alert(error));
    this.setState({ invitationLink: window.location.href + "/enroll" })
  }

  sendEmails(e) {
    e.preventDefault();
    axios.post("/tests/sendEmails",
      {
        "link": this.state.invitationLink,
        "receivers": this.state.receivers,
      }, { headers: { "Authorization": `Bearer ${localStorage["token"]}` } })
      .then((response) => alert(response.data.message)).catch(error => alert(error))
  }

  render() {
    return (
      <div className="Test">
        <h1>{this.state.subject}</h1>
        <h1> {this.state.name}</h1>

        <div className='TestFrame'>
          <h2>Початок тесту: {this.state.startDate.toLocaleString()}</h2>
          <h2>Кінець тесту: {this.state.finishDate.toLocaleString()}</h2>
          <h2>Тривалість: {this.state.duration} хв.</h2>
          <h2>Максимальна оцінка: {this.state.maxMark}</h2>
          {localStorage["isStudent"] == "true" ? <div>
          <h2>Ваша оцінка: {this.state.mark}</h2>
            <h2>Залишилось спроб: {this.state.numberOfAttempts}</h2>
            {((this.state.numberOfAttempts>0)&&(new Date(this.state.startDate)<new Date(Date.now()))
        &&(new Date(this.state.finishDate)>new Date(Date.now())))&&<div>
            <h2>Виберіть складність:</h2>
            <select className="Difficultyselect" value={this.state.difficulty} onChange={e => this.setDifficulty(e.target.value)}>
              <option value="1">Легко (60% від максимального балу)</option>
              <option value="2">Посередньо (80% від максимального балу)</option>
              <option value="3">Складно (100% від максимального балу)</option>
            </select><br></br>


            <button className='StartTestBtn' onClick={e => this.startTest(e)}>Почати тест</button>
            </div>}
          </div> : <div>{this.state.isOwner && <div>
            <div className='LinkInvite'>
              <h4>Запрошувальне посилання</h4>
              <button className="InviteBtn" onClick={this.copyToClipboard}>{this.state.copySuccess}</button>
              <input
                ref={(textarea) => this.textArea = textarea}
                value={this.state.invitationLink} readOnly
              />
            </div>
            <div className='MailInvite'>
              <h4>Введіть пошти студентів (кожну з нового рядка)</h4>
              <textarea value={this.state.receivers} onChange={e=>{e.preventDefault();this.setReceivers(e.target.value)}}></textarea><br></br><button className='InviteBtn' onClick={e => this.sendEmails(e)}>Надіслати запрошення</button></div></div>}</div>}

        </div>
      </div>
    );
  }
}
import axios from 'axios';
import React, { Component } from 'react';

export class Questions extends Component {
  static displayName = Questions.name;

  constructor(props) {
    super(props);
    this.state = { questions: [],
    sortby:"Name",
  sorttype:"asc",
limit:"10",
page:1,
search:"",
isNext:true, 
count:0 };
    }
setSortBy(sort)
{
  this.setState({sortby:sort})
}
setSearch(s)
{
  this.setState({search:s})
}
componentDidMount(){
  this.getQuestions(this.state.page)
}

addProduct(id){
  this.state.products.push(parseInt(id));
}
getQuestions(page)
{
  axios.get(`/questions?s=${this.state.search}&sort_by=${this.state.sortby}&sort_type=${this.state.sorttype}&offset=${page-1}&limit=${this.state.limit}`,
  { headers: {"Authorization" : `Bearer ${localStorage["token"]}`}})
  .then(response=>{this.setState({questions:response.data.questions,count:response.data.count})}).then(()=>{
    this.setState({page:page})
    if((this.state.count-(this.state.page)*this.state.limit)>0)
    {
      this.setState({isNext:true})
      
    }
    else{
      this.setState({isNext:false})
    }
  }).then(()=>{
    document.getElementById('Questions').innerHTML="";
    if(this.state.questions.length>0)
    {
  
    }
    for(var i =0;i<this.state.questions.length;i++)
    {
      var name = document.createElement("h3")
      name.innerText="Назва: "+this.state.questions[i].name
      var subject = document.createElement("h3")
      subject.innerText="Предмет: "+this.state.questions[i].subject
      var theme = document.createElement("h3")
      theme.innerText="Тема: "+ this.state.questions[i].theme
  
  
      var question = document.createElement("div");
      question.append(name,subject,theme)
      question.setAttribute("class","Question");
      question.setAttribute("id",`${this.state.questions[i].id}`)
      question.onclick = (e)=>{
        if(e.target.localName!=="div")
        {document.location.href=`/questions/${e.target.parentElement.id}`}
        else{
          document.location.href=`/questions/${e.target.id}`
        }
    } 
      document.getElementById('Questions').append(question)
    }
})
}
setSorttype(sort)
{
  this.setState({sorttype:sort})
}

setLimit(limit)
{
  this.setState({limit:limit})
}
Next()
{
  this.getQuestions(this.state.page+1)
  
}
Previous()
{
  this.getQuestions(this.state.page-1)
  
}
  

  render () {
    return (
      <div>
        <div class="MainFrame">
        <div className="Pagination">
        <div className="search">
              
            </div>
            <div className='InputField'>
            <h3>Знайти <input
                type="text"
                placeholder="Введіть текст для пошуку"
                name="search"
                onChange={e => this.setSearch(e.target.value)}
              /></h3></div>
          <h3>Сортувати по <select onChange={e=>{this.setSortBy(e.target.value)}}>
            <option value = "Name" >імені</option>
            <option value="Subject">предмету</option>
            <option value="Theme">темі</option>
          </select></h3>
          <h3>за 
          <select onChange={e=>{this.setSorttype(e.target.value)}}>
            <option value = "asc" >зростанням</option>
            <option value="desc" >спаданням</option>
          </select></h3>
         <div className='InputField'>
          <h3>Кількість на сторінці: <input className="InputField" type="number" min={1}value={this.state.limit} onChange={e=>{this.setLimit(e.target.value)}}placeholder="limit"></input></h3>
          <br></br>
          </div>
          <div className='InputField'>
          <button className = "filterButton" onClick={e=>{e.preventDefault();this.getQuestions(1)}}>Застосувати фільтри</button>
          </div>
        </div>
        
        <div id="Questions">
        </div>
        {(this.state.page>1)&&<button className="NavigationButton" onClick={e=>{e.preventDefault();this.Previous()}}>previous</button>}
        {this.state.isNext&&<button className="NavigationButton" onClick={e=>{e.preventDefault();this.Next()}}>next</button>}
        </div><div>{this.state.questions.length<1&&<div><h1 className="Warning">Питань не знайдено</h1></div>}
        </div>
      </div>
    );
  }
}

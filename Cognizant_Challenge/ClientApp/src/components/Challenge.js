import React, { Component } from 'react';

export class Challenge extends Component {
    static displayName = Challenge.name;

  constructor(props) {
    super(props);
      this.state = {
          name: "No name!",
          code: "No code!",
          task: "No task!"
      };
      this.handleNameChange = this.handleNameChange.bind(this);
      this.handleTaskChange = this.handleTaskChange.bind(this);
      this.handleCodeChange = this.handleCodeChange.bind(this);
      this.handleSubmit = this.handleSubmit.bind(this);
  }

   handleNameChange(event) {
        this.setState({ name: event.target.value });
    }
    handleTaskChange(event) {
        this.setState({ task: event.target.value });
    }
    handleCodeChange(event) {
        this.setState({ code: event.target.value });
    }


    handleSubmit(event) {
        event.preventDefault();

        fetch(process.env.REACT_APP_API + 'challenge', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type':'application/json'
            },
            body: JSON.stringify({
                Name: 'testName',
                ParticipantName: this.state.name,
                TaskName: this.state.task,
                SolutionCode: this.state.code,
            })
        })
        .then(res=>res.json())
        .then((result)=>{
            alert(result);
        },
        (error) => {
            alert('Failed');
        })
    }

  render() {
    return (
        <div>
            <form onSubmit={this.handleSubmit}>
                <div class="row">
                    <label class="column">NAME</label>
                    <input class="column2" type="text" onChange={this.handleNameChange} />
                </div>
                <div class="row">
                    <label class="column">SELECT TASK:</label>
                    <select class="column2" value={this.state.task} onChange={this.handleTaskChange}>
                        <option value="1">Task 1</option>
                        <option value="2">Task 2</option>
                        <option value="3">Task 3</option>
                        <option value="4">Task 4</option>
                    </select>
                </div>
                <div class="row">
                    <label class="column">DESCRIPTION</label>
                    <p class="column2" >Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam ornare tellus at urna bibendum, a malesuada urna tempus. Ut id augue ac lacus accumsan tincidunt. Curabitur dignissim nulla non iaculis convallis. Donec sem urna, rhoncus nec ligula nec, volutpat suscipit turpis. Ut sed dictum sem. Ut augue tellus, vestibulum sit amet accumsan condimentum, faucibus in augue. Etiam id tortor nec quam lobortis egestas. Praesent rutrum metus metus, at ultrices nibh condimentum vitae. Maecenas tempus, mi sed pulvinar egestas, augue est condimentum neque, quis convallis purus odio vel tellus. Phasellus lacus dui, pulvinar id mattis non, porttitor ac justo. Nunc placerat eu tortor ut interdum. Proin vel pharetra ligula, id consequat mi. Donec consectetur, nunc sed lacinia finibus, felis mi auctor nisi, ac blandit tortor risus eget ligula. Donec porttitor nunc at lorem scelerisque mattis. </p>
                </div>
                <div class="row">
                    <label class="column">SOLUTION CODE</label>
                    <textarea class="column2" type="text" onChange={this.handleCodeChange} />
                </div>
                <input type="submit" value="SUBMIT" />
          </form>

      </div>
    );
  }
}

module.exports  = {
    name:'infra:repositories',
    description:'',
    run:run
}

function run(toolbox){
    
    const nome = toolbox.parameters.first;

    if(typeof nome !== "string"){
        toolbox.print.error('First parameter not entered!')
        return;
    }

    
    toolbox.gera.generate({
        template: 'infra:irepositories.ejs',
        target: `src/LeilaoFake.Me.Infra/Datas/Repositories/I${nome}Repository.cs`,
        props: { 
            nome:nome,
        }
    })

    toolbox.gera.generate({
        template: 'infra:repositories.ejs',
        target: `src/LeilaoFake.Me.Infra/Datas/Repositories/${nome}Repository.cs`,
        props: { 
            nome:nome,
        }
    })
    

    const addTransient = `            services.AddTransient<I${nome}Repository,${nome}Repository>();`;

    toolbox.patching.patch('src/LeilaoFake.Me.Api/Startup.cs', { 
        insert: `\n${addTransient}`, 
        after: 'GERA-COMMANDS-ADD-REPOSITORY' 
    })

}

/*
    TOOLBOX PARAMETERS - toolbox.parameters.[name_field] :
        name	    type	    purpose	                                from the example above
        -------------------------------------------------------------------------------------------------
        plugin	    string	    the plugin used	                        'reactotron'
        command	    string	    the command used	                    'plugin'
        string	    string	    the command arguments as a string	    'MyAwesomePlugin full'
        array	    array	    the command arguments as an array	    ['MyAwesomePlugin', 'full']
        first	    string	    the 1st argument	                    'MyAwesomePlugin'
        second	    string	    the 2nd argument	                    'full'
        third	    string	    the 3rd argument	                    undefined
        options	    object	    command line options	                {comments: true, lint: 'standard'}
        argv	    object	    raw                                     argv	

*/

/*
    TOOLBOX TEMPLATE GENERATE - toolbox.template.generate({...}) 
        option	    type	    purpose	                                notes
        -----------------------------------------------------------------------------------------------------------
        template	string	    path to the EJS template	            relative from plugin's templates directory
        target	    string	    path to create the file	                relative from user's working directory
        props	    object	    more data to render in your template	
        directory	string	    where to find templates	                an absolute path (optional)
*/
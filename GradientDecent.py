import random
import numpy as np
import pandas as pd
def initialize(dim):
    b=random.random()
    theta=np.random.rand(dim)
    return b,theta
def predict_Y(b,theta,X):
    return b + np.dot(X,theta)
def get_cost(Y,Y_hat):
    Y_resd=Y-Y_hat
    return np.sum(np.dot(Y_resd.T,Y_resd))/len(Y-Y_resd)
def update_theta(x,y,y_hat,b_0,theta_o,learning_rate):
    db=(np.sum(y_hat-y)*2)/len(y)
    dw=(np.dot((y_hat-y),x)*2)/len(y)
    b_1=b_0-learning_rate*db
    theta_1=theta_o-learning_rate*dw
    return b_1,theta_1
def run_gradient_descent(X,Y,alpha,num_iterations):
    b,theta=initialize(X.shape[1])
    iter_num=0
    gd_iterations_df=pd.DataFrame(columns=['iteration','cost'])
    result_idx=0
    for each_iter in range(num_iterations):
        Y_hat=predict_Y(b,theta,X)
        this_cost=get_cost(Y,Y_hat)
        prev_b=b
        prev_theta=theta
        b,theta=update_theta(X,Y,Y_hat,prev_b,prev_theta,alpha)
        
        if(iter_num%10==0):
            gd_iterations_df.loc[result_idx]=[iter_num,this_cost]
            result_idx=result_idx+1
        iter_num +=1
    print('Final Estimate of b and theta : ',b,theta)
    return gd_iterations_df,b,theta
def eq(b,theta,dataset):
    theta_1=theta[0]
    theta_2=theta[1]
    theta_3=theta[2]
    theta_4=theta[3]
    theta_5=theta[4]
    theta_6=theta[5]
    
    dataset[['Temperature','Dew Point','Wind Speed','Humidity','Pressure','Condition_int']]
    h = b + theta_1 * dataset['Temperature'] + theta_2 * dataset['Dew Point'] +theta_3 * dataset['Wind Speed']+theta_4 * dataset['Humidity']+theta_5 * dataset['Pressure']+theta_5 * dataset['Condition_int']
    return h
dataset= pd.read_csv('Monthly.csv')
x = dataset[['Temperature','Dew Point','Wind Speed','Humidity','Pressure','Condition_int']]
y = dataset['N02BE']  #y true
# 0.13308661206584316 [-0.05841906  0.03431375 -0.03234194 -0.03291897  0.17413198  0.1080927 ]
# Temperature= content['Temperature'] 
# Dew_Point= content['Dew_Point']
# Wind_Speed= content['Wind_Speed'] 
# Humidity= content['Humidity']
# Pressure= content['Pressure']
# Condition_int=content['Condition_int']
Y=np.array((y-y.mean())/y.std())
X=x.apply(lambda rec:(rec-rec.mean())/rec.std(),axis=0)
b,theta=initialize(6)
print('Bias: ',b,'Weights: ',theta)
Y_hat=predict_Y(b,theta,X)
get_cost(Y,Y_hat)
Y_hat=predict_Y(b,theta,X)
Y_hat[0:10]
print("After initialization -Bias: ",b,"theta: ",theta)
Y_hat=predict_Y(b,theta,X)
b,theta=update_theta(X,Y,Y_hat,b,theta,0.01)
print("After first update -Bias: ",b,"theta: ",theta)
get_cost(Y,Y_hat)
gd_iterations_df,b,theta=run_gradient_descent(X,Y,alpha=0.001,num_iterations=1000)   
gd_iterations_df[0:3000]
print(gd_iterations_df)
Value=eq(b,theta,dataset)
print(Value)

from sklearn.metrics import mean_squared_error

# Given values
Y_true =Value # Y_true = Y (original values)

# calculated values
Y_pred = y # Y_pred = Y'

# Calculation of Mean Squared Error (MSE)
z=mean_squared_error(Y_true,Y_pred)
print(z)
print(z/301)

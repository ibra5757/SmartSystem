import pandas as pd
from sklearn.model_selection import GridSearchCV, cross_val_score, train_test_split
from sklearn.preprocessing import StandardScaler
from sklearn.linear_model import LogisticRegression
from sklearn.tree import DecisionTreeClassifier
from sklearn.ensemble import RandomForestClassifier, GradientBoostingClassifier
from sklearn.svm import SVC

# Load the dataset
df = pd.read_csv("DailyDataset_classified.csv")

X= df[['Year','Month','Day','Temperature','Dew Point','Humidity','Wind','Wind Speed','Pressure','IntCondition']]
y = df["Ro6Classification"]

# Split the data into training and testing sets
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=42)

# Standardize the features using StandardScaler
scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

# Define the classifiers to be tested and their hyperparameter grids
classifiers = {
    "Logistic Regression": (LogisticRegression(max_iter=5000), 
                            {'C': [0.01, 0.1, 1.0, 10, 100], 'penalty': ['l1', 'l2', 'elasticnet', 'none'],
                             'solver': ['newton-cg', 'lbfgs', 'liblinear', 'sag', 'saga'],
                             'class_weight': ['balanced', None]}),
    "Decision Tree": (DecisionTreeClassifier(), 
                      {'criterion': ['gini', 'entropy'], 'max_depth': [3, 5, None],
                       'min_samples_split': [2, 5, 10], 'min_samples_leaf': [1, 2, 4]}),
    "Random Forest": (RandomForestClassifier(), 
                      {'n_estimators': [50, 100, 150], 'max_depth': [3, 5, None],
                       'max_features': ['sqrt', 'log2'], 'min_samples_split': [2, 5, 10],
                       'min_samples_leaf': [1, 2, 4], 'bootstrap': [True, False]}),
    "SVM": (SVC(), 
            {'C': [0.1, 1, 10], 'kernel': ['linear', 'rbf'], 'gamma': ['scale', 'auto'],
             'class_weight': ['balanced', None]}),
    "Gradient Boosting": (GradientBoostingClassifier(), 
                          {'n_estimators': [50, 100, 200], 'max_depth': [3, 15, None],
                           'max_features': ['sqrt', 'log2'], 'learning_rate': [0.01, 0.1, 1],
                           'subsample': [0.5, 0.8, 1.0]})
}
# Perform cross-validation and hyperparameter tuning for each classifier
results = {}
for clf_name, (clf, param_grid) in classifiers.items():
    grid_search = GridSearchCV(clf, param_grid, cv=5)
    grid_search.fit(X_train_scaled, y_train)
    best_clf = grid_search.best_estimator_
    accuracy_scores = cross_val_score(best_clf, X_train_scaled, y_train, cv=10)
    results[clf_name] = {'Best Params': best_clf.get_params(), 
                         'Mean Accuracy': accuracy_scores.mean(),
                         'Accuracy STD': accuracy_scores.std()}
    results_df = pd.DataFrame.from_dict(results, orient='index')
    print(results_df)

# Convert the results dictionary to a pandas DataFrame
results_df = pd.DataFrame.from_dict(results, orient='index')

# Print the results DataFrame
print(results_df)
